using AutoMapper;
using MediatR;
using System.Xml.Linq;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Application.Publications.Queries;
using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.Import.Models;
using ZUEPC.Import.Parser;
using static ZUEPC.Import.Models.ImportPublication;

namespace ZUEPC.Application.Import.Services;

public class ImportService
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public ImportService(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}


	public void ImportFromCREPCXML(ImportCREPCXmlCommand command)
	{
		var result = ParseImportXMLCommand(command, OriginSourceType.CREPC);
		ProcessImportedRecords(result, OriginSourceType.CREPC);
	}

	public void ImportFromDaWinciXML(ImportDaWinciXmlCommand command)
	{
		var result = ParseImportXMLCommand(command, OriginSourceType.DAWINCI);
		ProcessImportedRecords(result, OriginSourceType.DAWINCI);
	}

	private void ProcessImportedRecords(IEnumerable<ImportRecord>? records, OriginSourceType source)
	{
		if (records is null || !records.Any())
		{
			return;
		};

		foreach (var item in records)
		{
			if(item is null)
			{
				continue;
			}
			var task = ProccesImportedRecordAsync(item, source);
			task.Wait();
		}
	}

	private async Task ProccesImportedRecordAsync(ImportRecord record, OriginSourceType source)
	{
		var publication = await FindOrCreatePublication(record.Publication, record.RecordVersionDate, source);
		if (publication.VersionDate <= record.RecordVersionDate)
		{
			await UpdateCurrentPublicationDetailsAsync(publication, record.Publication, record.RecordVersionDate, source);
		}
	}

	private async Task UpdateCurrentPublicationDetailsAsync(
		Publication currentPublication,
		ImportPublication importPublication, 
		DateTime versionDate,
		OriginSourceType source)
	{
		await UpdatePublicationBaseRecordAsync(currentPublication, importPublication, versionDate, source);
		await UpdatePublicationIdentifiersAsync(currentPublication, importPublication.PublicationIdentifiers, versionDate, source);

	}

	private async Task UpdatePublicationBaseRecordAsync(
		Publication currentPublication,
		ImportPublication importPublication,
		DateTime versionDate, OriginSourceType source)
	{

		var updateCommand = _mapper.Map<UpdatePublicationCommand>(importPublication);
		updateCommand.Id = currentPublication.Id;
		updateCommand.VersionDate = versionDate;
		updateCommand.OriginSourceType = source;

		await _mediator.Send(updateCommand);
	}

	private async Task UpdatePublicationIdentifiersAsync(
		Publication currentPublication, 
		IEnumerable<ImportPublicationIdentifier> importIdentifiers,
		DateTime versionDate, 
		OriginSourceType source)
	{

		var request = new GetAllPublicationIdentifiersQuery() { PublicationId = currentPublication.Id };
		var publicationCurrentIdentifiers = (await _mediator.Send(request)).PublicationIdentifiers;
		
		var allImportedIdentifiersString = importIdentifiers.Select(identifier => identifier.IdentifierValue).ToList();
		var allCurrentIdentifiersString = publicationCurrentIdentifiers.Select(identifier => identifier.IdentifierValue).ToList();

		var identifiersToDelete = publicationCurrentIdentifiers.Where(x=> x.VersionDate < versionDate && 
																          !allImportedIdentifiersString.Contains(x.IdentifierValue));


		await DeleteRecordsAsync<PublicationIdentifier, DeletePublicationIdentifierCommand>(identifiersToDelete);

		var importIdentifiersToInsert = importIdentifiers.Where(x => !allCurrentIdentifiersString.Contains(x.IdentifierValue));
		var identifiersToInsert = _mapper.Map<List<PublicationIdentifier>>(importIdentifiersToInsert);

		await InsertRecordsAsync<PublicationIdentifier, CreatePublicationCommand>(identifiersToInsert, versionDate, source);

		var identifiersTupleToUpdate =
			from current in publicationCurrentIdentifiers
			join import in importIdentifiers on current.IdentifierValue equals import.IdentifierValue
			where current.VersionDate < versionDate
			select new Tuple<PublicationIdentifier, ImportPublicationIdentifier>(current, import);

		await UpdateRecordsAsync<ImportPublicationIdentifier, PublicationIdentifier, UpdatePublicationIdentifierCommand>(
			  identifiersTupleToUpdate, 
			  versionDate, 
			  source);
	}

	private async Task DeleteRecordsAsync<TDomain, TCommand>(IEnumerable<TDomain> recordsToDelete)
		where TCommand : EPCDeleteBaseCommand, new()
		where TDomain : EPCBase
	{
		foreach (var record in recordsToDelete)
		{
			var deleteRequest = new TCommand() { Id = record.Id };
			await _mediator.Send(deleteRequest);
		}
	}


	private async Task InsertRecordsAsync<TDomain, TCommand>(IEnumerable<TDomain> recordsToInsert, DateTime versionDate, OriginSourceType source)
		where TCommand : EPCCreateBaseCommand, new()
		where TDomain : EPCBase
	{
		foreach (var record in recordsToInsert)
		{
			var insertRequest = _mapper.Map<TCommand>(record);
			insertRequest.VersionDate = versionDate;
			insertRequest.OriginSourceType = source;
			await _mediator.Send(insertRequest);
		}
	}

	private async Task UpdateRecordsAsync<TImport, TDomain, TCommand>(
		IEnumerable<Tuple<TDomain, TImport>> recordTuplesToUpdate,
		DateTime versionDate, 
		OriginSourceType source)
		where TCommand : EPCUpdateBaseCommand, new()
		where TDomain : EPCBase
	{

		foreach (var record in recordTuplesToUpdate)
		{
			var currentObject = record.Item1;
			var importObject = record.Item2;
				
			var updateRequest = _mapper.Map<TCommand>(importObject);
			updateRequest.VersionDate = versionDate;
			updateRequest.OriginSourceType = source;
			updateRequest.Id = currentObject.Id;
			await _mediator.Send(updateRequest);
		}
	}


	private async Task<Publication> FindOrCreatePublication(ImportPublication publicationRecord, DateTime versionDate, OriginSourceType source)
	{
		Publication? resultModel = null;
		long publicationId = -1;
		
		var imPublicationIdentifiers = publicationRecord.PublicationIdentifiers.Select(identifier => identifier.IdentifierValue);
		var foundPublicationIdentifiers = await _mediator.Send(new GetAllPublicationIdentifiersInSetQuery() 
		{
			SearchedIdentifiers = imPublicationIdentifiers
		});

		if (foundPublicationIdentifiers.Identifiers != null &&
			foundPublicationIdentifiers.Identifiers.Any())
		{
			publicationId = foundPublicationIdentifiers.Identifiers.First().PublicationId;
			resultModel = await GetPublicationById(publicationId);
			if (resultModel != null)
			{
				return resultModel;
			}
		}

		var imPublicationExternIds = publicationRecord.PublicationExternDbIds.Select(identifier => identifier.PublicationExternIdValue);
		var foundPublicationExternIdentifiers = await _mediator.Send(new GetAllPublicationExternDbIdsInSetQuery()
		{
			SearchedExternIdentifiers = imPublicationExternIds
		});

		if (foundPublicationExternIdentifiers.ExternDbIdentifiers != null &&
			foundPublicationExternIdentifiers.ExternDbIdentifiers.Any())
		{
			publicationId = foundPublicationExternIdentifiers.ExternDbIdentifiers.First().PublicationId;
			resultModel = await GetPublicationById(publicationId);
			if (resultModel != null)
			{
				return resultModel;
			}
		}

		return await CreatePublication(publicationRecord, versionDate ,source);
	}


	private async Task<Publication> CreatePublication(ImportPublication publication, DateTime versionDate ,OriginSourceType source)
	{
		var createCommand = _mapper.Map<CreatePublicationCommand>(publication);
		createCommand.VersionDate = versionDate;
		createCommand.OriginSourceType = source;


		return (await _mediator.Send(createCommand)).CreatedPublication;
	}

	private async Task<Publication?> GetPublicationById(long id)
	{
		var publicationResponse = await _mediator.Send(new GetPublicationQuery() { PublicationId = id});
		return publicationResponse.Publication;
	}

	private IEnumerable<ImportRecord>? ParseImportXMLCommand(ImportXmlBaseCommand command, OriginSourceType source)
	{
		if (command.XMLBody is null)
		{
			return null;
		}
		var doc = XDocument.Parse(command.XMLBody);
		if (source == OriginSourceType.CREPC)
		{
			return ImportParser.ParseCREPC(doc);
		}
		if (source == OriginSourceType.DAWINCI)
		{
			return ImportParser.ParseDaWinci(doc);
		}
		return null;
	}
}
