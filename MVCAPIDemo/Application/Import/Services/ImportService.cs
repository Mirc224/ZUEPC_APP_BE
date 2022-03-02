﻿using AutoMapper;
using MediatR;
using System.Xml.Linq;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.Import.Models;
using ZUEPC.Import.Models.Commond;
using ZUEPC.Import.Parser;

namespace ZUEPC.Application.Import.Services;

public partial class ImportService
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public ImportService(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}


	public async Task<ICollection<Publication>?> ImportFromCREPCXML(ImportCREPCXmlCommand command)
	{
		IEnumerable<ImportRecord>? result = ParseImportXMLCommand(command, OriginSourceType.CREPC);
		return await ProcessImportedRecords(result, OriginSourceType.CREPC);
	}

	public async Task<ICollection<Publication>?> ImportFromDaWinciXML(ImportDaWinciXmlCommand command)
	{
		IEnumerable<ImportRecord>? result = ParseImportXMLCommand(command, OriginSourceType.DAWINCI);
		return await ProcessImportedRecords(result, OriginSourceType.DAWINCI);
	}

	private IEnumerable<ImportRecord>? ParseImportXMLCommand(ImportXmlBaseCommand command, OriginSourceType source)
	{
		if (command.XMLBody is null)
		{
			return null;
		}
		XDocument? doc = XDocument.Parse(command.XMLBody);
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

	private async Task<ICollection<Publication>?> ProcessImportedRecords(IEnumerable<ImportRecord>? records, OriginSourceType source)
	{
		if (records is null || !records.Any())
		{
			return null;
		};
		List<Publication> importedList = new();
		foreach (ImportRecord? item in records)
		{
			if (item is null)
			{
				continue;
			}
			Publication newPublication = await ProccesImportedRecordAsync(item, source);
			importedList.Add(newPublication);
		}
		return importedList;
	}

	private async Task UpdateRecordsAsync<TDomain, TCommand>(
		IEnumerable<TDomain> recordsToUpdate,
		DateTime versionDate,
		OriginSourceType source)
		where TCommand : EPCUpdateBaseCommand, new()
		where TDomain : EPCBase
	{
		foreach (TDomain recordForUpdate in recordsToUpdate)
		{
			await UpdateRecordAsync<TDomain, TCommand>(recordForUpdate, versionDate, source);
		}
	}

	private async Task UpdateRecordAsync<TDomain, TCommand>(
		TDomain objectForUpdate,
		DateTime versionDate,
		OriginSourceType source)
		where TCommand : EPCUpdateBaseCommand, new()
		where TDomain : EPCBase
	{
		TCommand updateRequest = _mapper.Map<TCommand>(objectForUpdate);
		updateRequest.VersionDate = versionDate;
		updateRequest.OriginSourceType = source;
		await _mediator.Send(updateRequest);
	}

	private IEnumerable<Tuple<TImport, TDomain>> GetEPCObjectExternDatabaseIdsForUpdateAsync<
		TImport,
		TDomain>(
		IEnumerable<TImport> importExternIdentifiers,
		IEnumerable<TDomain> objectCurrentExternIds,
		DateTime versionDate)
		where TImport : EPCImportExternDatabaseIdBase
		where TDomain : EPCExternDatabaseIdBase
	{
		IEnumerable<Tuple<TImport, TDomain>> identifiersTupleToUpdate =
			from current in objectCurrentExternIds
			join import in importExternIdentifiers on current.ExternIdentifierValue equals import.ExternIdentifierValue
			where current.VersionDate < versionDate
			select new Tuple<TImport, TDomain>(import, current);

		return identifiersTupleToUpdate;
	}

	private IEnumerable<TImport> GetEPCObjectExternDatabaseIdsForInsertAsync<TImport, TDomain>(
		IEnumerable<TImport> importExternIdentifiers,
		IEnumerable<TDomain> objectCurrentExternIds)
		where TImport : EPCImportExternDatabaseIdBase
		where TDomain : EPCExternDatabaseIdBase
	{
		List<string> allCurrentExternIdsString = objectCurrentExternIds
			.Select(identifier => identifier.ExternIdentifierValue)
			.ToList();
		IEnumerable<TImport> importExternIdToInsert = importExternIdentifiers
			.Where(x => !allCurrentExternIdsString.Contains(x.ExternIdentifierValue));
		List<TDomain> identifiersToInsert = _mapper.Map<List<TDomain>>(importExternIdToInsert);

		return importExternIdToInsert;
	}
}
