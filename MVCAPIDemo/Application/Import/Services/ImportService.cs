using AutoMapper;
using MediatR;
using System.Xml.Linq;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Application.PublicationActivities.Commands;
using ZUEPC.Application.PublicationActivities.Queries;
using ZUEPC.Application.RelatedPublications.Commands;
using ZUEPC.Application.RelatedPublications.Queries;
using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;
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


	public async Task ImportFromCREPCXML(ImportCREPCXmlCommand command)
	{
		IEnumerable<ImportRecord>? result = ParseImportXMLCommand(command, OriginSourceType.CREPC);
		await ProcessImportedRecords(result, OriginSourceType.CREPC);
	}

	public async Task ImportFromDaWinciXML(ImportDaWinciXmlCommand command)
	{
		IEnumerable<ImportRecord>? result = ParseImportXMLCommand(command, OriginSourceType.DAWINCI);
		await ProcessImportedRecords(result, OriginSourceType.DAWINCI);
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

	private async Task ProcessImportedRecords(IEnumerable<ImportRecord>? records, OriginSourceType source)
	{
		if (records is null || !records.Any())
		{
			return;
		};

		foreach (ImportRecord? item in records)
		{
			if (item is null)
			{
				continue;
			}
			await ProccesImportedRecordAsync(item, source);
		}
	}

	private async Task ProcessPublicationActivitiesAsync(
		Publication currPublication, 
		IEnumerable<ImportPublicationActivity> publicationActivities, 
		DateTime versionDate, 
		OriginSourceType source)
	{
		IEnumerable<PublicationActivity> currPublicationActivities = (await _mediator.Send(
				new GetPublicationActivitiesCommand()
				{
					PublicationId = currPublication.Id
				})).PublicationActivities;

		IEnumerable<ImportPublicationActivity> activitiesToInsert = 
										from importPubActivity in publicationActivities
										where !(from currPubActivity in currPublicationActivities
										  where importPubActivity.Category == currPubActivity.Category &&
												importPubActivity.GovernmentGrant == currPubActivity.GovernmentGrant &&
												importPubActivity.ActivityYear == currPubActivity.ActivityYear
										  select 1).Any()
										select importPubActivity;

		List<PublicationActivity> mappedActivitiesToInsert = _mapper.Map<List<PublicationActivity>>(activitiesToInsert);
		foreach (PublicationActivity mappedActivity in mappedActivitiesToInsert)
		{
			mappedActivity.PublicationId = currPublication.Id;
		}
		await InsertRecordsAsync<PublicationActivity, CreatePublicationActivityCommand>(
			mappedActivitiesToInsert, 
			versionDate, 
			source);

		IEnumerable<Tuple<ImportPublicationActivity, PublicationActivity>> activityTuplesToUpdate =
										from importPubActivity in publicationActivities
										join currPubActivity in currPublicationActivities
										on 
										new
										{
											Category = importPubActivity.Category,
											GovernmentGrant = importPubActivity.GovernmentGrant,
											ActivityYear = importPubActivity.ActivityYear,
										}
										equals
										new
										{
											Category = currPubActivity.Category,
											GovernmentGrant = currPubActivity.GovernmentGrant,
											ActivityYear = currPubActivity.ActivityYear,
										}
										select new Tuple<ImportPublicationActivity, PublicationActivity>(
											importPubActivity,
											currPubActivity);

		foreach (Tuple<ImportPublicationActivity, PublicationActivity> tuple in activityTuplesToUpdate)
		{
			await UpdatePublicationActivityAsync(tuple.Item1, tuple.Item2, versionDate, source);
		}

	}

	private async Task UpdatePublicationActivityAsync(
		ImportPublicationActivity importRecord, 
		PublicationActivity currRecord, 
		DateTime versionDate, 
		OriginSourceType source)
	{
		PublicationActivity recordForUpdate = _mapper.Map<PublicationActivity>(importRecord);
		recordForUpdate.PublicationId = currRecord.PublicationId;
		await UpdateRecordAsync<PublicationActivity, UpdatePublicationActivityCommand>(
				recordForUpdate,
				versionDate,
				source);
	}

	private async Task ProccesImportedRecordAsync(ImportRecord record, OriginSourceType source)
	{
		ImportPublication importedPublication = record.Publication;
		DateTime versionDate = record.RecordVersionDate;

		await ProcessImportedPublication(importedPublication, versionDate, source);
	}

	private async Task DeleteRecordsAsync<TDomain, TCommand>(IEnumerable<TDomain> recordsToDelete)
		where TCommand : EPCDeleteBaseCommand, new()
		where TDomain : EPCBase
	{
		foreach (TDomain record in recordsToDelete)
		{
			TCommand deleteRequest = new TCommand() { Id = record.Id };
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
		foreach (TDomain record in recordsToInsert)
		{
			await InsertRecordAsync<TDomain, TCommand>(record, versionDate, source);
		}
	}

	private async Task InsertRecordAsync<TDomain, TCommand>(
		TDomain recordToInsert,
		DateTime versionDate,
		OriginSourceType source)
		where TCommand : EPCCreateBaseCommand, new()
		where TDomain : EPCBase
	{
		TCommand insertRequest = _mapper.Map<TCommand>(recordToInsert);
		insertRequest.VersionDate = versionDate;
		insertRequest.OriginSourceType = source;
		await _mediator.Send(insertRequest);
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
		List<string> allImportedExternIdsString = importExternIdentifiers.Select(identifier => identifier.ExternIdentifierValue).ToList();
		List<string> allCurrentExternIdsString = objectCurrentExternIds.Select(identifier => identifier.ExternIdentifierValue).ToList();

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
