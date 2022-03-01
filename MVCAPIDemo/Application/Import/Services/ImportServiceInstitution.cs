using ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Commands.InstitutionNames;
using ZUEPC.Application.Institutions.Commands.Institutions;
using ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.Application.Institutions.Queries.Institutions;
using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportInstitution;

namespace ZUEPC.Application.Import.Services;

public partial class ImportService
{
	private async Task<IEnumerable<Tuple<ImportInstitution, Institution>>> ProcessImportInstitutions(IEnumerable<ImportInstitution> relatedInstitutions, DateTime versionDate, OriginSourceType source)
	{
		IEnumerable<Tuple<ImportInstitution, Institution>> publicationInstitutionTuples =
			await GetOrCreatePublicationInstitutionImportDomainTuplesAsync(
																		   relatedInstitutions,
																		   versionDate,
																		   source);

		IEnumerable<Tuple<ImportInstitution, Institution>> updatedCurrentInstitutions = await UpdateCurrentInstitutionsDetailsAsync(
																								publicationInstitutionTuples,
																								versionDate,
																								source);
		return updatedCurrentInstitutions;
	}

	private async Task<IEnumerable<Tuple<ImportInstitution, Institution>>> GetOrCreatePublicationInstitutionImportDomainTuplesAsync(
	IEnumerable<ImportInstitution> relatedInstitutions,
	DateTime versionDate,
	OriginSourceType source)
	{
		List<Tuple<ImportInstitution, Institution>> publicationInstitutionImportDomainTuples = new();
		foreach (ImportInstitution importInstitution in relatedInstitutions)
		{
			Institution domainInstitution = await FindOrCreateInstitution(importInstitution, versionDate, source);
			publicationInstitutionImportDomainTuples.Add(new Tuple<ImportInstitution, Institution>(importInstitution, domainInstitution));
		}

		return publicationInstitutionImportDomainTuples;
	}

	private async Task<Institution> FindOrCreateInstitution(ImportInstitution importInstitution, DateTime versionDate, OriginSourceType source)
	{
		Institution? resultModel = null;
		long institutionId = -1;

		List<string> institutionExternDatabaseIds = importInstitution
														.InstitutionExternDatabaseIds
														.Select(x => x.ExternIdentifierValue)
														.ToList();
		GetAllInstitutionExternDbIdsInSetQueryResponse foundInstitutionExternIdentifiers = await _mediator.Send(
			new GetAllInstitutionExternDbIdsInSetQuery()
			{
				SearchedExternIdentifiers = institutionExternDatabaseIds
			});

		if (foundInstitutionExternIdentifiers.ExternDatabaseIds != null &&
			foundInstitutionExternIdentifiers.ExternDatabaseIds.Any())
		{
			institutionId = foundInstitutionExternIdentifiers.ExternDatabaseIds.First().Id;
			resultModel = await GetInstitutionByIdAsync(institutionId);
			if (resultModel != null)
			{
				return resultModel;
			}
		}

		return await CreateInstitutionAsync(importInstitution, versionDate, source);
	}

	private async Task<IEnumerable<Tuple<ImportInstitution, Institution>>> UpdateCurrentInstitutionsDetailsAsync(
		IEnumerable<Tuple<ImportInstitution, Institution>> publicationInstitutionTuples,
		DateTime versionDate,
		OriginSourceType source)
	{
		List<Tuple<ImportInstitution, Institution>> updatedInstitutions = new();
		foreach (Tuple<ImportInstitution, Institution> institutionTuple in publicationInstitutionTuples)
		{
			ImportInstitution importInstitution = institutionTuple.Item1;
			Institution currentInstitution = institutionTuple.Item2;
			Institution updatedInstitution = _mapper.Map<Institution>(importInstitution);
			updatedInstitution.Id = currentInstitution.Id;
			updatedInstitutions.Add(new Tuple<ImportInstitution, Institution>(importInstitution, updatedInstitution));
			if (versionDate == currentInstitution.VersionDate)
			{
				continue;
			}
			await UpdateCurrentInstitutionDetailsAsync(currentInstitution, importInstitution, versionDate, source);
		}
		return updatedInstitutions;
	}

	private async Task UpdateCurrentInstitutionDetailsAsync(
		Institution currentInstitution,
		ImportInstitution importInstitution,
		DateTime versionDate,
		OriginSourceType source)
	{
		await UpdateInstitutionBaseRecordAsync(
			currentInstitution,
			importInstitution,
			versionDate,
			source);
		await UpdateInstitutionExternDatabaseIdDataAsync(
			currentInstitution,
			importInstitution.InstitutionExternDatabaseIds,
			versionDate,
			source);
		await UpdateInstitutionNameDataAsync(
			currentInstitution,
			importInstitution.InstitutionNames,
			versionDate,
			source);
	}

	private async Task UpdateInstitutionNameDataAsync(
		Institution currentInstitution,
		IEnumerable<ImportInstitutionName> importInstitutionNames,
		DateTime versionDate,
		OriginSourceType source)
	{
		GetInstitutionNamesQuery request = new() { InstitutionId = currentInstitution.Id };
		IEnumerable<InstitutionName> institutionCurrentNames = (await _mediator.Send(request)).InstitutionNames;

		List<string> allImportedNamesString = importInstitutionNames.Select(identifier => identifier.Name).ToList();
		List<string> allCurrentNamesString = institutionCurrentNames.Select(identifier => identifier.Name).ToList();

		IEnumerable<ImportInstitutionName> namesToInsert = from institutionImpName in importInstitutionNames
														   where !(from institutionCurrName in institutionCurrentNames
																   where institutionCurrName.Name == institutionImpName.Name &&
																		 institutionCurrName.NameType == institutionImpName.NameType
																   select 1).Any()
														   select institutionImpName;

		List<InstitutionName> mappedNamesToInsert = _mapper.Map<List<InstitutionName>>(namesToInsert);

		await InsertRecordsAsync<InstitutionName, CreateInstitutionNameCommand>(mappedNamesToInsert, versionDate, source);

		IEnumerable<Tuple<ImportInstitutionName, InstitutionName>> nameTuplesToUpdate = from institutionCurrName in institutionCurrentNames
																						join institutionImpName in importInstitutionNames on
																						new { Name = institutionCurrName.Name, NameType = institutionCurrName.NameType }
																						equals
																						new { Name = institutionImpName.Name, NameType = institutionImpName.NameType }
																						where institutionCurrName.VersionDate < versionDate
																						select new Tuple<ImportInstitutionName, InstitutionName>(
																							institutionImpName,
																							institutionCurrName);
		
		foreach (Tuple<ImportInstitutionName, InstitutionName> nameTuple in nameTuplesToUpdate)
		{
			await UpdateInstitutionNameAsync(nameTuple.Item1, nameTuple.Item2, versionDate, source);
		}
	}

	private async Task UpdateInstitutionNameAsync(
		ImportInstitutionName importInstitutionName,
		InstitutionName currInstitutionName,
		DateTime versionDate,
		OriginSourceType source)
	{
		InstitutionName recordForUpdate = _mapper.Map<InstitutionName>(importInstitutionName);
		recordForUpdate.InstitutionId = currInstitutionName.InstitutionId;
		await UpdateRecordAsync<InstitutionName, UpdateInstitutionNameCommand>(
			  recordForUpdate,
			  versionDate,
			  source);
	}



	private async Task UpdateInstitutionExternDatabaseIdDataAsync(
		Institution currentInstitution,
		IEnumerable<ImportInstitutionExternDatabaseId> importExternDatabaseIds,
		DateTime versionDate,
		OriginSourceType source)
	{
		GetInstitutionExternDatabaseIdsQuery request = new() { InstitutionId = currentInstitution.Id };
		IEnumerable<InstitutionExternDatabaseId> institutionCurrentExternIds = (await _mediator.Send(request))
			.InstitutionExternDatabaseIds;

		IEnumerable<ImportInstitutionExternDatabaseId> importExternIdToInsert = GetEPCObjectExternDatabaseIdsForInsertAsync(
																					importExternDatabaseIds,
																					institutionCurrentExternIds);

		List<InstitutionExternDatabaseId> identifiersToInsert =
			_mapper.Map<List<InstitutionExternDatabaseId>>(importExternIdToInsert);

		foreach (InstitutionExternDatabaseId externIdentifier in identifiersToInsert)
		{
			externIdentifier.InstitutionId = currentInstitution.Id;
		}

		await InsertRecordsAsync<InstitutionExternDatabaseId, CreateInstitutionExternDatabaseIdCommand>(
			identifiersToInsert,
			versionDate,
			source);

		IEnumerable<Tuple<ImportInstitutionExternDatabaseId, InstitutionExternDatabaseId>> recordTuplesForUpdate =
			GetEPCObjectExternDatabaseIdsForUpdateAsync(
				importExternDatabaseIds,
				institutionCurrentExternIds,
				versionDate);

		foreach (Tuple<ImportInstitutionExternDatabaseId, InstitutionExternDatabaseId> tuple in recordTuplesForUpdate)
		{
			await UpdateInstitutionExternDatabaseIdAsync(tuple.Item1, tuple.Item2, versionDate, source);
		}
	}

	private async Task UpdateInstitutionExternDatabaseIdAsync(
		ImportInstitutionExternDatabaseId importRecord,
		InstitutionExternDatabaseId currentRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		InstitutionExternDatabaseId recordForUpdate = _mapper.Map<InstitutionExternDatabaseId>(importRecord);
		recordForUpdate.InstitutionId = currentRecord.Id;
		await UpdateRecordAsync<InstitutionExternDatabaseId, UpdateInstitutionExternDatabaseIdCommand>(
				recordForUpdate,
				versionDate,
				source);
	}

	private async Task UpdateInstitutionBaseRecordAsync(
		Institution currentInstitution,
		ImportInstitution importInstitution,
		DateTime versionDate,
		OriginSourceType source)
	{
		if (currentInstitution.VersionDate < versionDate)
		{
			Institution publicationForUpdate = _mapper.Map<Institution>(importInstitution);
			await UpdateRecordAsync<Institution, UpdateInstitutionCommand>(
				publicationForUpdate,
				versionDate,
				source);
		}
	}

	private async Task<Institution> CreateInstitutionAsync(ImportInstitution importInstitution, DateTime versionDate, OriginSourceType source)
	{
		CreateInstitutionCommand createCommand = _mapper.Map<CreateInstitutionCommand>(importInstitution);
		createCommand.VersionDate = versionDate;
		createCommand.OriginSourceType = source;
		return (await _mediator.Send(createCommand)).Institution;
	}

	private async Task<Institution?> GetInstitutionByIdAsync(long institutionId)
	{
		GetInstitutionQueryResponse? response = await _mediator.Send(
			new GetInstitutionQuery()
			{
				InstitutionId = institutionId
			});
		return response.Institution;
	}
}
