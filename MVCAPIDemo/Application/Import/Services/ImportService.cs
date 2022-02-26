using AutoMapper;
using MediatR;
using System.Xml.Linq;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Queries.Persons;
using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Persons;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.Import.Models;
using ZUEPC.Import.Models.Commond;
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
		await UpdateCurrentPublicationDetailsAsync(publication, record.Publication, record.RecordVersionDate, source);
		var relatedPersons = record.Publication.PublicationAuthors.Select(x => x.Person).ToList();
		var publicationPersons = await GetUpdatedOrCreatedPublicationPersonImportDomainTuples(relatedPersons, record.RecordVersionDate, source);
	}

	private async Task<IEnumerable<Tuple<ImportPerson, Person>>> GetUpdatedOrCreatedPublicationPersonImportDomainTuples(
		IEnumerable<ImportPerson> relatedPersons, 
		DateTime recordVersion, 
		OriginSourceType source)
	{
		var publicationPersonImporDomainTuples = new List<Tuple<ImportPerson, Person>>();
		foreach (var importPerson in relatedPersons)
		{
			var domainPerson = await FindOrCreatePerson(importPerson, recordVersion, source);
			publicationPersonImporDomainTuples.Add(new Tuple<ImportPerson, Person>(importPerson, domainPerson));
		}

		return publicationPersonImporDomainTuples;
	}

	private async Task<Person> FindOrCreatePerson(ImportPerson importPerson, DateTime recordVersion, OriginSourceType source)
	{
		Person? resultModel = null;
		long personId = -1;

		var personExternDatabaseIds = importPerson.PersonExternDatabaseIds.Select(x => x.ExternIdentifierValue).ToList();
		var foundPersonExternIdentifiers = await _mediator.Send(new GetAllPersonsExternDbIdsInSetQuery()
		{
			SearchedExternIdentifiers = personExternDatabaseIds
		});
		
		if (foundPersonExternIdentifiers.ExternDatabaseIds != null &&
			foundPersonExternIdentifiers.ExternDatabaseIds.Any())
		{
			personId = foundPersonExternIdentifiers.ExternDatabaseIds.First().Id;
			resultModel = await GetPersonById(personId);
			if (resultModel != null)
			{
				return resultModel;
			}
		}

		return await CreatePerson(importPerson, recordVersion, source);
	}

	private async Task<Person?> GetPersonById(long id)
	{
		var personResponse = await _mediator.Send(new GetPersonQuery() { PersonId = id });
		return personResponse.Person;
	}

	private async Task<Person> CreatePerson(ImportPerson importPerson, DateTime versionDate, OriginSourceType source)
	{
		var createCommand = _mapper.Map<CreatePersonCommand>(importPerson);
		createCommand.VersionDate = versionDate;
		createCommand.OriginSourceType = source;

		return (await _mediator.Send(createCommand)).Person;
	}

	private async Task UpdateCurrentPublicationDetailsAsync(
		Publication currentPublication,
		ImportPublication importPublication, 
		DateTime versionDate,
		OriginSourceType source)
	{
		await UpdatePublicationBaseRecordAsync(currentPublication, importPublication, versionDate, source);
		await UpdatePublicationIdentifiersAsync(currentPublication, importPublication.PublicationIdentifiers, versionDate, source);
		await UpdatePublicationExternDatabaseIdsAsync(currentPublication, importPublication.PublicationExternDbIds, versionDate, source);
		await UpdatePublicationNamesAsync(currentPublication, importPublication.PublicationNames, versionDate, source);
	}


	private async Task UpdatePublicationNamesAsync(
		Publication currentPublication, 
		IEnumerable<ImportPublicationName> importPublicationNames,
		DateTime versionDate, 
		OriginSourceType source)
	{
		var request = new GetAllPublicationNamesQuery() { PublicationId = currentPublication.Id };
		var publicationCurrentNames = (await _mediator.Send(request)).PublicationNames;

		var allImportedNamesString = importPublicationNames.Select(identifier => identifier.Name).ToList();
		var allCurrentNamesString = publicationCurrentNames.Select(identifier => identifier.Name).ToList();

		var namesToDelete = from publicationCurrName in publicationCurrentNames
								  where publicationCurrName.VersionDate < versionDate &&
								  !(from publicationImpName in importPublicationNames
									where publicationImpName.Name == publicationCurrName.Name &&
										  publicationImpName.NameType == publicationCurrName.NameType
									select 1).Any()
								  select publicationCurrName;
		await DeleteRecordsAsync<PublicationName, DeletePublicationNameCommand>(namesToDelete);

		var namesToInsert = from publicationImpName in importPublicationNames
							where !(from publicationCurrName in publicationCurrentNames
									where publicationCurrName.Name == publicationImpName.Name &&
										  publicationCurrName.NameType == publicationImpName.NameType
									select 1).Any()
							select publicationImpName;

		var mappedNamesToInsert = _mapper.Map<List<PublicationName>>(namesToInsert);

		await InsertRecordsAsync<PublicationName, CreatePublicationNameCommand>(mappedNamesToInsert, versionDate, source);

		var nameTuplesToUpdate = from publicationCurrName in publicationCurrentNames
								 join publicationImpName in importPublicationNames on
								 new { Name = publicationCurrName.Name, NameType = publicationCurrName.NameType}
								 equals
								 new { Name = publicationImpName.Name, NameType = publicationImpName.NameType }
								 where publicationCurrName.VersionDate < versionDate 
								 select new Tuple<ImportPublicationName, PublicationName>(publicationImpName, publicationCurrName);

		await UpdateRecordsAsync<ImportPublicationName, PublicationName, UpdatePublicationNameCommand>(
			  nameTuplesToUpdate,
			  versionDate,
			  source);
	}

	private async Task UpdatePublicationBaseRecordAsync(
		Publication currentPublication,
		ImportPublication importPublication,
		DateTime versionDate, 
		OriginSourceType source)
	{
		if (currentPublication.VersionDate < versionDate)
		{
			await UpdateRecordAsync<ImportPublication, Publication, UpdatePublicationCommand>(
				currentPublication,
				importPublication,
				versionDate,
				source);
		}
		
		//var updateCommand = _mapper.Map<UpdatePublicationCommand>(importPublication);
		//updateCommand.Id = currentPublication.Id;
		//updateCommand.VersionDate = versionDate;
		//updateCommand.OriginSourceType = source;

		//await _mediator.Send(updateCommand);
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
			select new Tuple<ImportPublicationIdentifier, PublicationIdentifier>(import, current);

		await UpdateRecordsAsync<ImportPublicationIdentifier, PublicationIdentifier, UpdatePublicationIdentifierCommand>(
			  identifiersTupleToUpdate, 
			  versionDate, 
			  source);
	}


	private async Task UpdatePublicationExternDatabaseIdsAsync(
		Publication currentPublication,
		IEnumerable<ImportPublicationExternDatabaseId> importExternIdentifiers,
		DateTime versionDate,
		OriginSourceType source)
	{
		var request = new GetAllPublicationExternDatabaseIdsQuery() { PublicationId = currentPublication.Id };
		var publicationCurrentExternIds = (await _mediator.Send(request)).PublicationExternDatabaseIds;

		await UpdateEPCObjectExternDatabaseIdsAsync<
			ImportPublicationExternDatabaseId,
			PublicationExternDatabaseId,
			CreatePublicationExternDatabaseIdCommand,
			DeletePublicationExternDbIdCommand,
			UpdatePublicationExternDatabaseIdCommand>(publicationCurrentExternIds, importExternIdentifiers, versionDate, source);


		//var allImportedExternIdsString = importExternIdentifiers.Select(identifier => identifier.ExternIdentifierValue).ToList();
		//var allCurrentExternIdsString = publicationCurrentExternIds.Select(identifier => identifier.ExternIdentifierValue).ToList();

		//var identifiersToDelete = publicationCurrentExternIds.Where(x => x.VersionDate < versionDate &&
		//																  !allImportedExternIdsString.Contains(x.ExternIdentifierValue));

		//await DeleteRecordsAsync<PublicationExternDatabaseId, DeletePublicationExternDbIdCommand>(identifiersToDelete);

		//var importIdentifiersToInsert = importExternIdentifiers.Where(x => !allCurrentExternIdsString.Contains(x.ExternIdentifierValue));
		//var identifiersToInsert = _mapper.Map<List<PublicationExternDatabaseId>>(importIdentifiersToInsert);

		//await InsertRecordsAsync<PublicationExternDatabaseId, CreatePublicationExternDatabaseIdCommand>(identifiersToInsert, versionDate, source);

		//var identifiersTupleToUpdate =
		//	from current in publicationCurrentExternIds
		//	join import in importExternIdentifiers on current.ExternIdentifierValue equals import.ExternIdentifierValue
		//	where current.VersionDate < versionDate
		//	select new Tuple<ImportPublicationExternDatabaseId, PublicationExternDatabaseId>(import, current);

		//await UpdateRecordsAsync<ImportPublicationExternDatabaseId, PublicationExternDatabaseId, UpdatePublicationExternDatabaseIdCommand>(
		//	  identifiersTupleToUpdate,
		//	  versionDate,
		//	  source);
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

		var imPublicationExternIds = publicationRecord.PublicationExternDbIds.Select(identifier => identifier.ExternIdentifierValue);
		var foundPublicationExternIdentifiers = await _mediator.Send(new GetAllPublicationExternDbIdsInSetQuery()
		{
			SearchedExternIdentifiers = imPublicationExternIds
		});

		if (foundPublicationExternIdentifiers.ExternDatabaseIds != null &&
			foundPublicationExternIdentifiers.ExternDatabaseIds.Any())
		{
			publicationId = foundPublicationExternIdentifiers.ExternDatabaseIds.First().PublicationId;
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

	private async Task InsertRecordsAsync<TDomain, TCommand>(
		IEnumerable<TDomain> recordsToInsert,
		DateTime versionDate,
		OriginSourceType source)
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
		IEnumerable<Tuple<TImport, TDomain>> recordTuplesToUpdate,
		DateTime versionDate,
		OriginSourceType source)
		where TCommand : EPCUpdateBaseCommand, new()
		where TDomain : EPCBase
	{

		foreach (var record in recordTuplesToUpdate)
		{
			var importObject = record.Item1;
			var currentObject = record.Item2;

			await UpdateRecordAsync<TImport, TDomain, TCommand>(currentObject, importObject, versionDate, source);
		}
	}

	private async Task UpdateRecordAsync<TImport, TDomain, TCommand>(
		TDomain currentObject,
		TImport importObject,
		DateTime versionDate,
		OriginSourceType source)
		where TCommand : EPCUpdateBaseCommand, new()
		where TDomain : EPCBase
	{
		var updateRequest = _mapper.Map<TCommand>(importObject);
		updateRequest.VersionDate = versionDate;
		updateRequest.OriginSourceType = source;
		updateRequest.Id = currentObject.Id;
		await _mediator.Send(updateRequest);
	}

	private async Task UpdateEPCObjectExternDatabaseIdsAsync<
		TImport,
		TDomain,
		TCreateCommand,
		TDeleteCommand,
		TUpdateCommand>(
		IEnumerable<TDomain> objectCurrentExternIds,
		IEnumerable<TImport> importExternIdentifiers,
		DateTime versionDate,
		OriginSourceType source)
		where TImport : EPCImportExternDatabaseIdBase
		where TDomain : EPCExternDatabaseIdBase
		where TCreateCommand : EPCCreateBaseCommand, new()
		where TDeleteCommand : EPCDeleteBaseCommand, new()
		where TUpdateCommand : EPCUpdateBaseCommand, new()
	{

		var allImportedExternIdsString = importExternIdentifiers.Select(identifier => identifier.ExternIdentifierValue).ToList();
		var allCurrentExternIdsString = objectCurrentExternIds.Select(identifier => identifier.ExternIdentifierValue).ToList();

		var identifiersToDelete = objectCurrentExternIds.Where(x => x.VersionDate < versionDate &&
																	!allImportedExternIdsString.Contains(x.ExternIdentifierValue));

		await DeleteRecordsAsync<TDomain, TDeleteCommand>(identifiersToDelete);

		var importExternIdToInsert = importExternIdentifiers.Where(x => !allCurrentExternIdsString.Contains(x.ExternIdentifierValue));
		var identifiersToInsert = _mapper.Map<List<TDomain>>(importExternIdToInsert);

		await InsertRecordsAsync<TDomain, TCreateCommand>(identifiersToInsert, versionDate, source);

		var identifiersTupleToUpdate =
			from current in objectCurrentExternIds
			join import in importExternIdentifiers on current.ExternIdentifierValue equals import.ExternIdentifierValue
			where current.VersionDate < versionDate
			select new Tuple<TImport, TDomain>(import, current);

		await UpdateRecordsAsync<TImport, TDomain, TUpdateCommand>(
			  identifiersTupleToUpdate,
			  versionDate,
			  source);
	}
}
