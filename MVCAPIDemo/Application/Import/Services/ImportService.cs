using AutoMapper;
using MediatR;
using System.Xml.Linq;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Application.Publications.Queries;
using ZUEPC.Base.Enums.Common;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.Import.Models;
using ZUEPC.Import.Parser;

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
			var task = ProccesImportedRecord(item, source);
			task.Wait();
		}
	}

	private async Task ProccesImportedRecord(ImportRecord record, OriginSourceType source)
	{
		var publicationModel = await FindOrCreatePublication(record.Publication, source);
	}

	private async Task<PublicationModel> FindOrCreatePublication(ImportPublication publication, OriginSourceType source)
	{
		PublicationModel? resultModel = null;
		long publicationId = -1;
		
		var imPublicationIdentifiers = publication.PublicationIdentifiers.Select(identifier => identifier.PublicationIdentifierValue);
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

		var imPublicationExternIds = publication.PublicationExternDbIds.Select(identifier => identifier.PublicationExternIdValue);
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



		return new PublicationModel();
	}

	private async Task<PublicationModel?> GetPublicationById(long id)
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
