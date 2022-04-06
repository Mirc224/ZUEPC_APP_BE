using AutoMapper;
using MediatR;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Base.Commands;
using ZUEPC.Base.Domain;
using ZUEPC.Base.Enums.Common;
using ZUEPC.Base.Extensions;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.EvidencePublication.Domain.Publications;
using ZUEPC.Import.Models;
using ZUEPC.Import.Models.Common;
using ZUEPC.Import.Parser;

namespace ZUEPC.Application.Import.Services;

public partial class ImportService
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public ImportService(
		IMapper mapper, 
		IMediator mediator
		)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public async Task<IEnumerable<Publication>?> ImportFromCREPCXML(ImportCREPCXmlCommand command)
	{
		IEnumerable<ImportRecord>? result = ParseImportXMLCommand(command, OriginSourceType.CREPC);
		return await ProcessImportedRecords(result, OriginSourceType.CREPC);
	}

	public async Task<IEnumerable<Publication>?> ImportFromDaWinciXML(ImportDaWinciXmlCommand command)
	{
		IEnumerable<ImportRecord>? result = ParseImportXMLCommand(command, OriginSourceType.DAWINCI);
		return await ProcessImportedRecords(result, OriginSourceType.DAWINCI);
	}

	private IEnumerable<ImportRecord>? ParseImportXMLCommand(ImportXmlBaseCommand command, OriginSourceType source)
	{
		if (command.XEelementBody is null)
		{
			return null;
		}
		if (source == OriginSourceType.CREPC)
		{
			return ImportParser.ParseCREPC(command.XEelementBody);
		}
		if (source == OriginSourceType.DAWINCI)
		{
			return ImportParser.ParseDaWinci(command.XEelementBody);
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

	private async Task<Publication> ProccesImportedRecordAsync(ImportRecord record, OriginSourceType source)
	{
		ImportPublication importedPublication = record.Publication;
		DateTime versionDate = record.RecordVersionDate;

		return await ProcessImportedPublication(importedPublication, versionDate, source);
	}


	private async Task UpdateRecordsAsync<TDomain, TCommand>(
		IEnumerable<TDomain> recordsToUpdate,
		DateTime versionDate,
		OriginSourceType source)
		where TCommand : EPCUpdateCommandBase, new()
		where TDomain : EPCDomainBase
	{
		foreach (TDomain recordForUpdate in recordsToUpdate.OrEmptyIfNull())
		{
			await UpdateRecordAsync<TDomain, TCommand>(recordForUpdate, versionDate, source);
		}
	}

	private async Task UpdateRecordAsync<TDomain, TCommand>(
		TDomain objectForUpdate,
		DateTime versionDate,
		OriginSourceType source)
		where TCommand : EPCUpdateCommandBase, new()
		where TDomain : EPCDomainBase
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
		where TDomain : EPCDomainBase, IEPCItemWithExternIdentifier, IEPCItemBase
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
		where TDomain : EPCDomainBase, IEPCItemWithExternIdentifier
	{
		List<string> allCurrentExternIdsString = objectCurrentExternIds
			.Select(identifier => identifier.ExternIdentifierValue)
			.ToList();
		IEnumerable<TImport> importExternIdToInsert = importExternIdentifiers
			.Where(x => !allCurrentExternIdsString.Contains(x.ExternIdentifierValue));
		List<TDomain> identifiersToInsert = _mapper.Map<List<TDomain>>(importExternIdToInsert);

		return importExternIdToInsert;
	}

	private async Task DeleteRecordsAsync<TDomain, TCommand>(IEnumerable<TDomain> recordsToDelete)
	where TCommand : EPCDeleteModelCommandBase<long>, new()
	where TDomain : EPCDomainBase
	{
		foreach (TDomain record in recordsToDelete.OrEmptyIfNull())
		{
			TCommand deleteRequest = new TCommand() { Id = record.Id };
			await _mediator.Send(deleteRequest);
		}
	}

	private async Task InsertRecordsAsync<TDomain, TCommand>(
		IEnumerable<TDomain> recordsToInsert,
		DateTime versionDate,
		OriginSourceType source)
		where TCommand : EPCCreateCommandBase, new()
		where TDomain : EPCDomainBase
	{
		foreach (TDomain record in recordsToInsert.OrEmptyIfNull())
		{
			await InsertRecordAsync<TDomain, TCommand>(record, versionDate, source);
		}
	}

	private async Task InsertRecordAsync<TDomain, TCommand>(
		TDomain recordToInsert,
		DateTime versionDate,
		OriginSourceType source)
		where TCommand : EPCCreateCommandBase, new()
		where TDomain : EPCDomainBase
	{
		TCommand insertRequest = _mapper.Map<TCommand>(recordToInsert);
		insertRequest.VersionDate = versionDate;
		insertRequest.OriginSourceType = source;
		await _mediator.Send(insertRequest);
	}
}
