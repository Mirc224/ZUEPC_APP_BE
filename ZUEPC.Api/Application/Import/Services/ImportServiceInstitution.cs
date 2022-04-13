using ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Commands.InstitutionNames;
using ZUEPC.Application.Institutions.Commands.Institutions;
using ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.Application.Institutions.Queries.Institutions;
using ZUEPC.Base.Enums.Common;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.Institutions;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportInstitution;

namespace ZUEPC.Application.Import.Services;

public partial class ImportService
{
	private async Task<IEnumerable<Tuple<ImportInstitution, Institution>>> ProcessImportInstitutionsCollectionAsync(
		IEnumerable<ImportInstitution> relatedInstitutions,
		DateTime versionDate,
		OriginSourceType source)
	{

		List<Tuple<ImportInstitution, Institution>> processedInstitutionsTuples = new();
		foreach (ImportInstitution institution in relatedInstitutions)
		{
			Institution updatedInstitution = await ProcessImportInstitutionAsync(institution, versionDate, source);
			processedInstitutionsTuples.Add(new Tuple<ImportInstitution, Institution>(institution, updatedInstitution));
		}
		return processedInstitutionsTuples;
	}

	private async Task<Institution> ProcessImportInstitutionAsync(
		ImportInstitution importInstitution,
		DateTime versionDate,
		OriginSourceType source)
	{
		Institution instituton = await FindOrCreateInstitutionAsync(importInstitution, versionDate, source);
		if (instituton.VersionDate < versionDate)
		{
			instituton = await UpdateInstitutionBaseAsync(importInstitution, instituton, versionDate, source);
		}
		await UpdateInstitutionExternDatabaseIdDataAsync(
				instituton,
				importInstitution.InstitutionExternDatabaseIds,
				versionDate,
				source);
		await UpdateInstitutionNameDataAsync(instituton, importInstitution.InstitutionNames, versionDate, source);

		return instituton;
	}

	private async Task<Institution> UpdateInstitutionBaseAsync(
		ImportInstitution importInstitution,
		Institution instituton,
		DateTime versionDate,
		OriginSourceType source)
	{
		Institution updatedInstitution = _mapper.Map<Institution>(importInstitution);
		updatedInstitution.Id = instituton.Id;
		updatedInstitution.VersionDate = versionDate;
		updatedInstitution.OriginSourceType = source;
		await UpdateRecordAsync<Institution, UpdateInstitutionCommand>(updatedInstitution, versionDate, source);
		return updatedInstitution;
	}

	private async Task<IEnumerable<Tuple<ImportInstitution, Institution>>> GetOrCreatePublicationInstitutionImportDomainTuplesAsync(
	IEnumerable<ImportInstitution> relatedInstitutions,
	DateTime versionDate,
	OriginSourceType source)
	{
		List<Tuple<ImportInstitution, Institution>> publicationInstitutionImportDomainTuples = new();
		foreach (ImportInstitution importInstitution in relatedInstitutions)
		{
			Institution domainInstitution = await FindOrCreateInstitutionAsync(importInstitution, versionDate, source);
			publicationInstitutionImportDomainTuples.Add(new Tuple<ImportInstitution, Institution>(
				importInstitution,
				domainInstitution));
		}

		return publicationInstitutionImportDomainTuples;
	}

	private async Task<Institution> FindOrCreateInstitutionAsync(
		ImportInstitution importInstitution,
		DateTime versionDate,
		OriginSourceType source)
	{
		Institution? resultModel = null;
		long institutionId = -1;

		List<string> institutionExternDatabaseIds = importInstitution
														.InstitutionExternDatabaseIds
														.Select(x => x.ExternIdentifierValue)
														.ToList();
		GetAllInstitutionExternDbIdsInSetQueryResponse foundInstitutionExternIdentifiers = null;
		if (institutionExternDatabaseIds.Any())
		{
			foundInstitutionExternIdentifiers = await _mediator.Send(
			new GetAllInstitutionExternDbIdsInSetQuery()
			{
				SearchedExternIdentifiers = institutionExternDatabaseIds
			});
		}

		if (foundInstitutionExternIdentifiers?.Data != null &&
			foundInstitutionExternIdentifiers.Data.Any())
		{
			institutionId = foundInstitutionExternIdentifiers
							.Data
							.GroupBy(x => x.InstitutionId)
							.OrderByDescending(x => x.Count())
							.First()
							.First()
							.InstitutionId;
			resultModel = await GetInstitutionByIdAsync(institutionId);
			if (resultModel != null)
			{
				return resultModel;
			}
		}

		return await CreateInstitutionAsync(importInstitution, versionDate, source);
	}

	private async Task UpdateInstitutionNameDataAsync(
		Institution currentInstitution,
		IEnumerable<ImportInstitutionName> importInstitutionNames,
		DateTime versionDate,
		OriginSourceType source)
	{
		GetInstitutionInstitutionNamesQuery request = new() { InstitutionId = currentInstitution.Id };
		IEnumerable<InstitutionName> institutionCurrentNames = (await _mediator.Send(request)).Data;
		IEnumerable<ImportInstitutionName> namesToInsert = from institutionImpName in importInstitutionNames
														   where !(from institutionCurrName in institutionCurrentNames
																   where institutionCurrName.Name == institutionImpName.Name &&
																		 institutionCurrName.NameType == institutionImpName.NameType
																   select 1).Any()
														   select institutionImpName;

		await InsertInstitutionNameCollectionAsync(currentInstitution, namesToInsert, versionDate, source);

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

	private async Task InsertInstitutionNameCollectionAsync(
		Institution currentInstitution,
		IEnumerable<ImportInstitutionName> importInstitutionNames,
		DateTime versionDate,
		OriginSourceType source)
	{
		foreach (ImportInstitutionName name in importInstitutionNames.OrEmptyIfNull())
		{
			if (name.Name is null)
			{
				continue;
			}
			await InsertInstitutionNameAsync(currentInstitution, name, versionDate, source);
		}
	}

	private async Task InsertInstitutionNameAsync(
		Institution currentInstitution,
		ImportInstitutionName importInstitutionName,
		DateTime versionDate,
		OriginSourceType source)
	{
		InstitutionName nameToInsert = _mapper.Map<InstitutionName>(importInstitutionName);
		nameToInsert.InstitutionId = currentInstitution.Id;
		await InsertRecordAsync<InstitutionName, CreateInstitutionNameCommand>(nameToInsert, versionDate, source);
	}

	private async Task UpdateInstitutionNameAsync(
		ImportInstitutionName importInstitutionName,
		InstitutionName currInstitutionName,
		DateTime versionDate,
		OriginSourceType source)
	{
		InstitutionName recordForUpdate = _mapper.Map<InstitutionName>(importInstitutionName);
		recordForUpdate.Id = currInstitutionName.Id;
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
		GetInstitutionInstitutionExternDatabaseIdsQuery request = new() { InstitutionId = currentInstitution.Id };
		IEnumerable<InstitutionExternDatabaseId> institutionCurrentExternIds = (await _mediator.Send(request)).Data;

		IEnumerable<ImportInstitutionExternDatabaseId> importExternIdToInsert = GetEPCObjectExternDatabaseIdsForInsertAsync(
																					importExternDatabaseIds,
																					institutionCurrentExternIds);

		await InsertInstitutionExternDatabaseIdCollectionAsync(
			currentInstitution,
			importExternIdToInsert,
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

	private async Task InsertInstitutionExternDatabaseIdCollectionAsync(
		Institution currentInstitution,
		IEnumerable<ImportInstitutionExternDatabaseId> importInstitutionExternDbIds,
		DateTime versionDate,
		OriginSourceType source)
	{
		foreach (ImportInstitutionExternDatabaseId identifier in importInstitutionExternDbIds)
		{
			if (identifier.ExternIdentifierValue is null)
			{
				continue;
			}
			await InsertInstitutionExternDatabaseIdAsync(currentInstitution, identifier, versionDate, source);
		}
	}

	private async Task InsertInstitutionExternDatabaseIdAsync(
		Institution currentInstitution,
		ImportInstitutionExternDatabaseId importInstitutionExternDbId,
		DateTime versionDate,
		OriginSourceType source)
	{
		InstitutionExternDatabaseId identifierToInsert = _mapper.Map<InstitutionExternDatabaseId>(importInstitutionExternDbId);
		identifierToInsert.InstitutionId = currentInstitution.Id;
		await InsertRecordAsync<InstitutionExternDatabaseId, CreateInstitutionExternDatabaseIdCommand>(
			identifierToInsert,
			versionDate,
			source);
	}

	private async Task UpdateInstitutionExternDatabaseIdAsync(
		ImportInstitutionExternDatabaseId importRecord,
		InstitutionExternDatabaseId currentRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		InstitutionExternDatabaseId recordForUpdate = _mapper.Map<InstitutionExternDatabaseId>(importRecord);
		recordForUpdate.Id = currentRecord.Id;
		recordForUpdate.InstitutionId = currentRecord.InstitutionId;
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
			publicationForUpdate.Id = currentInstitution.Id;
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
		Institution newInstitution = (await _mediator.Send(createCommand)).Data;
		await InsertInstitutionExternDatabaseIdCollectionAsync(
			newInstitution,
			importInstitution.InstitutionExternDatabaseIds,
			versionDate,
			source);
		await InsertInstitutionNameCollectionAsync(
			newInstitution,
			importInstitution.InstitutionNames,
			versionDate,
			source);
		return newInstitution;
	}

	private async Task<Institution?> GetInstitutionByIdAsync(long institutionId)
	{
		GetInstitutionQueryResponse? response = await _mediator.Send(
			new GetInstitutionQuery()
			{
				Id = institutionId
			});
		return response.Data;
	}
}
