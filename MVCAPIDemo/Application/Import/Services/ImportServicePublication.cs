using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportPublication;

namespace ZUEPC.Application.Import.Services;

public partial class ImportService
{
	private async Task<Publication> ProcessImportedPublication(
		ImportPublication importedPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		Publication publication = await FindOrCreatePublication(importedPublication, versionDate, source);
		Publication updatedPublication = await UpdateCurrentPublicationDetailsAsync(publication,
																					importedPublication,
																					versionDate,
																					source);
		await ProcessImportPublicationAuthorsAsync(updatedPublication, importedPublication.PublicationAuthors, versionDate, source);


		await ProcessRelatedPublicationsAsync(updatedPublication, importedPublication.RelatedPublications, versionDate, source);
		
		await ProcessPublicationActivitiesAsync(updatedPublication, importedPublication.PublicationActivities, versionDate, source);

		return updatedPublication;
	}

	private async Task<Publication> UpdateCurrentPublicationDetailsAsync(
		Publication currentPublication,
		ImportPublication importPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		Publication updatedPublication = _mapper.Map<Publication>(importPublication);
		updatedPublication.Id = currentPublication.Id;
		updatedPublication.VersionDate = versionDate;
		updatedPublication.OriginSourceType = source;
		await UpdatePublicationBaseRecordAsync(updatedPublication, versionDate, source);
		await UpdatePublicationIdentifierDataAsync(updatedPublication, importPublication.PublicationIdentifiers, versionDate, source);
		await UpdatePublicationExternDatabaseIdDataAsync(updatedPublication, importPublication.PublicationExternDbIds, versionDate, source);
		await UpdatePublicationNameDataAsync(updatedPublication, importPublication.PublicationNames, versionDate, source);
		return updatedPublication;
	}

	private async Task UpdatePublicationBaseRecordAsync(
		Publication currentPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		if (currentPublication.VersionDate < versionDate)
		{
			await UpdateRecordAsync<Publication, UpdatePublicationCommand>(
				currentPublication,
				versionDate,
				source);
		}
	}

	private async Task<Publication> FindOrCreatePublication(ImportPublication publicationRecord, DateTime versionDate, OriginSourceType source)
	{
		Publication? resultModel = null;
		long publicationId = -1;

		IEnumerable<string> imPublicationIdentifiers = publicationRecord.PublicationIdentifiers.Select(identifier => identifier.IdentifierValue);
		GetAllPublicationIdentifiersInSetQueryResponse foundPublicationIdentifiers = await _mediator.Send(new GetAllPublicationIdentifiersInSetQuery()
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

		IEnumerable<string> imPublicationExternIds = publicationRecord.PublicationExternDbIds.Select(identifier => identifier.ExternIdentifierValue);
		GetAllPublicationExternDbIdsInSetQueryResponse foundPublicationExternIdentifiers = await _mediator.Send(new GetAllPublicationExternDbIdsInSetQuery()
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

		return await CreatePublication(publicationRecord, versionDate, source);
	}

	private async Task UpdatePublicationIdentifierDataAsync(
		Publication currentPublication,
		IEnumerable<ImportPublicationIdentifier> importIdentifiers,
		DateTime versionDate,
		OriginSourceType source)
	{

		GetAllPublicationIdentifiersQuery request = new() { PublicationId = currentPublication.Id };
		IEnumerable<PublicationIdentifier> publicationCurrentIdentifiers = (await _mediator.Send(request)).PublicationIdentifiers;

		List<string> allImportedIdentifiersString = importIdentifiers.Select(identifier => identifier.IdentifierValue).ToList();
		List<string?> allCurrentIdentifiersString = publicationCurrentIdentifiers.Select(identifier => identifier.IdentifierValue).ToList();

		IEnumerable<ImportPublicationIdentifier> importIdentifiersToInsert = importIdentifiers.Where(x => !allCurrentIdentifiersString.Contains(x.IdentifierValue));
		List<PublicationIdentifier> identifiersToInsert = _mapper.Map<List<PublicationIdentifier>>(importIdentifiersToInsert);
		foreach (PublicationIdentifier identifier in identifiersToInsert)
		{
			identifier.PublicationId = currentPublication.Id;
		}
		await InsertRecordsAsync<PublicationIdentifier, CreatePublicationIdentifierCommand>(identifiersToInsert, versionDate, source);

		IEnumerable<Tuple<ImportPublicationIdentifier, PublicationIdentifier>> identifiersTupleToUpdate =
			from current in publicationCurrentIdentifiers
			join import in importIdentifiers on current.IdentifierValue equals import.IdentifierValue
			where current.VersionDate < versionDate
			select new Tuple<ImportPublicationIdentifier, PublicationIdentifier>(import, current);

		foreach (Tuple<ImportPublicationIdentifier, PublicationIdentifier> tuple in identifiersTupleToUpdate)
		{
			await UpdatePublicationIdentifierAsync(tuple.Item1, tuple.Item2, versionDate, source);
		}
	}

	private async Task UpdatePublicationIdentifierAsync(
		ImportPublicationIdentifier importRecord,
		PublicationIdentifier currRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		PublicationIdentifier recordForUpdate = _mapper.Map<PublicationIdentifier>(importRecord);
		recordForUpdate.PublicationId = currRecord.Id;
		await UpdateRecordAsync<PublicationIdentifier, UpdatePublicationIdentifierCommand>(
				recordForUpdate,
				versionDate,
				source);
	}

	private async Task UpdatePublicationExternDatabaseIdDataAsync(
		Publication currentPublication,
		IEnumerable<ImportPublicationExternDatabaseId> importExternIdentifiers,
		DateTime versionDate,
		OriginSourceType source)
	{
		GetPublicationExternDatabaseIdsQuery request = new() { PublicationId = currentPublication.Id };
		IEnumerable<PublicationExternDatabaseId> publicationCurrentExternIds = (await _mediator.Send(request))
																				.PublicationExternDatabaseIds;

		IEnumerable<ImportPublicationExternDatabaseId> importExternIdToInsert = GetEPCObjectExternDatabaseIdsForInsertAsync(
			importExternIdentifiers,
			publicationCurrentExternIds);

		List<PublicationExternDatabaseId> identifiersToInsert = _mapper.Map<List<PublicationExternDatabaseId>>(importExternIdToInsert);

		foreach (PublicationExternDatabaseId externIdentifier in identifiersToInsert)
		{
			externIdentifier.PublicationId = currentPublication.Id;
		}

		await InsertRecordsAsync<PublicationExternDatabaseId, CreatePublicationExternDatabaseIdCommand>
			(identifiersToInsert, versionDate, source);

		IEnumerable<Tuple<ImportPublicationExternDatabaseId, PublicationExternDatabaseId>> recordTuplesForUpdate = GetEPCObjectExternDatabaseIdsForUpdateAsync(
			importExternIdentifiers,
			publicationCurrentExternIds,
			versionDate);

		foreach (Tuple<ImportPublicationExternDatabaseId, PublicationExternDatabaseId> tuple in recordTuplesForUpdate)
		{
			await UpdatePublicationExternDatabaseIdAsync(tuple.Item1, tuple.Item2, versionDate, source);
		}
	}

	private async Task UpdatePublicationExternDatabaseIdAsync(
		ImportPublicationExternDatabaseId importRecord,
		PublicationExternDatabaseId currRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		PublicationExternDatabaseId recordForUpdate = _mapper.Map<PublicationExternDatabaseId>(importRecord);
		recordForUpdate.PublicationId = currRecord.Id;
		await UpdateRecordAsync<PublicationExternDatabaseId, UpdatePublicationExternDatabaseIdCommand>(
				recordForUpdate,
				versionDate,
				source);
	}

	private async Task UpdatePublicationNameDataAsync(
		Publication currentPublication,
		IEnumerable<ImportPublicationName> importPublicationNames,
		DateTime versionDate,
		OriginSourceType source)
	{
		GetPublicationNamesQuery request = new() { PublicationId = currentPublication.Id };
		IEnumerable<PublicationName> publicationCurrentNames = (await _mediator.Send(request)).PublicationNames;

		List<string> allImportedNamesString = importPublicationNames.Select(identifier => identifier.Name).ToList();
		List<string> allCurrentNamesString = publicationCurrentNames.Select(identifier => identifier.Name).ToList();

		IEnumerable<ImportPublicationName> namesToInsert = from publicationImpName in importPublicationNames
														   where !(from publicationCurrName in publicationCurrentNames
																   where publicationCurrName.Name == publicationImpName.Name &&
																		 publicationCurrName.NameType == publicationImpName.NameType
																   select 1).Any()
														   select publicationImpName;

		List<PublicationName> mappedNamesToInsert = _mapper.Map<List<PublicationName>>(namesToInsert);

		await InsertRecordsAsync<PublicationName, CreatePublicationNameCommand>(mappedNamesToInsert, versionDate, source);

		IEnumerable<Tuple<ImportPublicationName, PublicationName>> nameTuplesToUpdate = from publicationCurrName in publicationCurrentNames
																						join publicationImpName in importPublicationNames on
																						new
																						{
																							Name = publicationCurrName.Name,
																							NameType = publicationCurrName.NameType
																						}
																						equals
																						new
																						{
																							Name = publicationImpName.Name,
																							NameType = publicationImpName.NameType
																						}
																						where publicationCurrName.VersionDate < versionDate
																						select new Tuple<ImportPublicationName, PublicationName>(publicationImpName, publicationCurrName);

		foreach (Tuple<ImportPublicationName, PublicationName> tuple in nameTuplesToUpdate)
		{
			await UpdatePublicationNameAsync(tuple.Item1, tuple.Item2, versionDate, source);
		}
	}

	private async Task UpdatePublicationNameAsync(
		ImportPublicationName importRecord,
		PublicationName currRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		PublicationName recordForUpdate = _mapper.Map<PublicationName>(importRecord);
		recordForUpdate.PublicationId = currRecord.Id;
		await UpdateRecordAsync<PublicationName, UpdatePublicationNameCommand>(
				recordForUpdate,
				versionDate,
				source);
	}

	private async Task<Publication> CreatePublication(ImportPublication publication, DateTime versionDate, OriginSourceType source)
	{
		CreatePublicationCommand createCommand = _mapper.Map<CreatePublicationCommand>(publication);
		createCommand.VersionDate = versionDate;
		createCommand.OriginSourceType = source;
		return (await _mediator.Send(createCommand)).Publication;
	}

	private async Task<Publication?> GetPublicationById(long id)
	{
		GetPublicationQueryResponse publicationResponse = await _mediator.Send(new GetPublicationQuery() { PublicationId = id });
		return publicationResponse.Publication;
	}
}
