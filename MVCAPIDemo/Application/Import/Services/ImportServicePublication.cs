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
		if (publication.VersionDate < versionDate)
		{
			publication = await UpdatePublicationBaseAsync(publication, importedPublication, versionDate, source);
		}
		await UpdatePublicationIdentifierDataAsync(publication, importedPublication.PublicationIdentifiers, versionDate, source);
		await UpdatePublicationExternDatabaseIdDataAsync(publication, importedPublication.PublicationExternDbIds, versionDate, source);
		await UpdatePublicationNameDataAsync(publication, importedPublication.PublicationNames, versionDate, source);

		await ProcessImportPublicationAuthorsAsync(publication, importedPublication.PublicationAuthors, versionDate, source);
		await ProcessRelatedPublicationsAsync(publication, importedPublication.RelatedPublications, versionDate, source);
		await ProcessPublicationActivitiesAsync(publication, importedPublication.PublicationActivities, versionDate, source);

		return publication;
	}

	private async Task<Publication> UpdatePublicationBaseAsync(
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
		return updatedPublication;
	}

	private async Task UpdatePublicationBaseRecordAsync(
		Publication currentPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		await UpdateRecordAsync<Publication, UpdatePublicationCommand>(
			currentPublication,
			versionDate,
			source);
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

		GetPublicationPublicationIdentifiersQuery request = new() { PublicationId = currentPublication.Id };
		IEnumerable<PublicationIdentifier>? publicationCurrentIdentifiers = (await _mediator.Send(request)).PublicationIdentifiers;
		if (publicationCurrentIdentifiers is null)
		{
			return;
		}
		List<string?> allCurrentIdentifiersString = publicationCurrentIdentifiers.Select(identifier => identifier.IdentifierValue).ToList();

		IEnumerable<ImportPublicationIdentifier> importIdentifiersToInsert = importIdentifiers
			.Where(x => !allCurrentIdentifiersString.Contains(x.IdentifierValue));

		await InsertPublicationIdentifierCollectionAsync(currentPublication, importIdentifiersToInsert, versionDate, source);

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

	private async Task InsertPublicationIdentifierCollectionAsync(
		Publication currentPublication,
		IEnumerable<ImportPublicationIdentifier> importIdentifiersToInsert,
		DateTime versionDate,
		OriginSourceType source)
	{
		foreach (ImportPublicationIdentifier identifier in importIdentifiersToInsert)
		{
			if(identifier.IdentifierValue is null)
			{
				continue;
			}
			await InsertPublicationIdentifierAsync(currentPublication, identifier, versionDate, source);
		}
	}

	private async Task InsertPublicationIdentifierAsync(
		Publication currentPublication,
		ImportPublicationIdentifier importIdentifierToInsert,
		DateTime versionDate,
		OriginSourceType source)
	{
		PublicationIdentifier identifierToInsert = _mapper.Map<PublicationIdentifier>(importIdentifierToInsert);
		identifierToInsert.PublicationId = currentPublication.Id;
		await InsertRecordAsync<PublicationIdentifier, CreatePublicationIdentifierCommand>(identifierToInsert, versionDate, source);
	}

	private async Task UpdatePublicationIdentifierAsync(
		ImportPublicationIdentifier importRecord,
		PublicationIdentifier currRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		PublicationIdentifier recordForUpdate = _mapper.Map<PublicationIdentifier>(importRecord);
		recordForUpdate.Id = currRecord.Id;
		recordForUpdate.PublicationId = currRecord.PublicationId;
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
		GetPublicationPublicationExternDatabaseIdsQuery request = new() { PublicationId = currentPublication.Id };
		IEnumerable<PublicationExternDatabaseId> publicationCurrentExternIds = (await _mediator.Send(request))
																				.PublicationExternDatabaseIds;

		IEnumerable<ImportPublicationExternDatabaseId> importExternIdsToInsert = GetEPCObjectExternDatabaseIdsForInsertAsync(
			importExternIdentifiers,
			publicationCurrentExternIds);

		await InsertPublicationExternDatabaseIdCollectionAsync(
				currentPublication,
				importExternIdsToInsert,
				versionDate,
				source);

		IEnumerable<Tuple<ImportPublicationExternDatabaseId, PublicationExternDatabaseId>> recordTuplesForUpdate = GetEPCObjectExternDatabaseIdsForUpdateAsync(
			importExternIdentifiers,
			publicationCurrentExternIds,
			versionDate);

		foreach (Tuple<ImportPublicationExternDatabaseId, PublicationExternDatabaseId> tuple in recordTuplesForUpdate)
		{
			await UpdatePublicationExternDatabaseIdAsync(tuple.Item1, tuple.Item2, versionDate, source);
		}
	}

	private async Task InsertPublicationExternDatabaseIdCollectionAsync(
		Publication currentPublication,
		IEnumerable<ImportPublicationExternDatabaseId> importExternIdsToInsert,
		DateTime versionDate,
		OriginSourceType source)
	{
		foreach (ImportPublicationExternDatabaseId externIdentifier in importExternIdsToInsert)
		{
			if (externIdentifier.ExternIdentifierValue is null)
			{
				continue;
			}
			await InsertPublicationExternDatabaseIdAsync(currentPublication, externIdentifier, versionDate, source);
		}
	}

	private async Task InsertPublicationExternDatabaseIdAsync(
		Publication currentPublication,
		ImportPublicationExternDatabaseId importExternIdentifier,
		DateTime versionDate,
		OriginSourceType source)
	{
		PublicationExternDatabaseId identifierToInsert = _mapper.Map<PublicationExternDatabaseId>(importExternIdentifier);
		identifierToInsert.PublicationId = currentPublication.Id;
		await InsertRecordAsync<PublicationExternDatabaseId, CreatePublicationExternDatabaseIdCommand>(
			identifierToInsert,
			versionDate,
			source);

	}

	private async Task UpdatePublicationExternDatabaseIdAsync(
		ImportPublicationExternDatabaseId importRecord,
		PublicationExternDatabaseId currRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		PublicationExternDatabaseId recordForUpdate = _mapper.Map<PublicationExternDatabaseId>(importRecord);
		recordForUpdate.PublicationId = currRecord.PublicationId;
		recordForUpdate.Id = currRecord.Id;
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
		GetPublicationPublicationNamesQuery request = new() { PublicationId = currentPublication.Id };
		IEnumerable<PublicationName> publicationCurrentNames = (await _mediator.Send(request)).PublicationNames;

		List<string> allImportedNamesString = importPublicationNames.Select(identifier => identifier.Name).ToList();
		List<string> allCurrentNamesString = publicationCurrentNames.Select(identifier => identifier.Name).ToList();

		IEnumerable<ImportPublicationName> namesToInsert = from publicationImpName in importPublicationNames
														   where !(from publicationCurrName in publicationCurrentNames
																   where publicationCurrName.Name == publicationImpName.Name &&
																		 publicationCurrName.NameType == publicationImpName.NameType
																   select 1).Any()
														   select publicationImpName;

		await InsertPublicationNameCollectionAsync(currentPublication, namesToInsert, versionDate, source);

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

	private async Task InsertPublicationNameCollectionAsync(
		Publication currentPublication,
		IEnumerable<ImportPublicationName> importNames,
		DateTime versionDate,
		OriginSourceType source)
	{
		foreach (ImportPublicationName name in importNames)
		{
			if(name.Name is null && name.NameType is null)
			{
				continue;
			}
			await InsertPublicationNameAsync(currentPublication, name, versionDate, source);
		}
	}

	public async Task InsertPublicationNameAsync(
		Publication currentPublication,
		ImportPublicationName importName,
		DateTime versionDate,
		OriginSourceType source)
	{
		PublicationName nameToInsert = _mapper.Map<PublicationName>(importName);
		nameToInsert.PublicationId = currentPublication.Id;
		await InsertRecordAsync<PublicationName, CreatePublicationNameCommand>(nameToInsert, versionDate, source);
	}

	private async Task UpdatePublicationNameAsync(
		ImportPublicationName importRecord,
		PublicationName currRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		PublicationName recordForUpdate = _mapper.Map<PublicationName>(importRecord);
		recordForUpdate.PublicationId = currRecord.PublicationId;
		recordForUpdate.Id = currRecord.Id;
		await UpdateRecordAsync<PublicationName, UpdatePublicationNameCommand>(
				recordForUpdate,
				versionDate,
				source);
	}

	private async Task<Publication> CreatePublication(ImportPublication importPublication, DateTime versionDate, OriginSourceType source)
	{
		CreatePublicationCommand createCommand = _mapper.Map<CreatePublicationCommand>(importPublication);
		createCommand.VersionDate = versionDate;
		createCommand.OriginSourceType = source;
		Publication currentPublication = (await _mediator.Send(createCommand)).Publication;
		await InsertPublicationIdentifierCollectionAsync(
				currentPublication,
				importPublication.PublicationIdentifiers,
				versionDate,
				source);
		await InsertPublicationExternDatabaseIdCollectionAsync(
				currentPublication,
				importPublication.PublicationExternDbIds,
				versionDate,
				source);
		return currentPublication;
	}

	private async Task<Publication?> GetPublicationById(long id)
	{
		GetPublicationQueryResponse publicationResponse = await _mediator.Send(new GetPublicationQuery() { PublicationId = id });
		return publicationResponse.Publication;
	}
}
