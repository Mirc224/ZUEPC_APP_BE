using ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Commands.PersonNames;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Queries.PersonNames;
using ZUEPC.Application.Persons.Queries.Persons;
using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Domain.Persons;
using ZUEPC.Import.Models;
using static ZUEPC.Import.Models.ImportPerson;

namespace ZUEPC.Application.Import.Services;

public partial class ImportService
{
	private async Task<IEnumerable<Tuple<ImportPerson, Person>>> ProcessImportPersonsAsync(
		IEnumerable<ImportPerson> relatedPersons,
		DateTime versionDate,
		OriginSourceType source)
	{
		IEnumerable<Tuple<ImportPerson, Person>> publicationPersonsTuples = await GetOrCreatePublicationPersonImportDomainTuplesAsync(
																					relatedPersons,
																					versionDate,
																					source);

		IEnumerable<Tuple<ImportPerson, Person>> updatedCurrentPersonsTuples = await UpdateCurrentPersonsDetailsAsync(
																						publicationPersonsTuples,
																						versionDate,
																						source);

		return updatedCurrentPersonsTuples;
	}

	private async Task<IEnumerable<Tuple<ImportPerson, Person>>> UpdateCurrentPersonsDetailsAsync(
	IEnumerable<Tuple<ImportPerson, Person>> publicationPersonsTuples,
	DateTime versionDate,
	OriginSourceType source)
	{
		List<Tuple<ImportPerson, Person>> updatedPersons = new();
		foreach (Tuple<ImportPerson, Person> personTuple in publicationPersonsTuples)
		{
			ImportPerson importPerson = personTuple.Item1;
			Person currentPerson = personTuple.Item2;
			Person updatedPerson = _mapper.Map<Person>(importPerson);
			updatedPerson.Id = currentPerson.Id;

			updatedPersons.Add(new Tuple<ImportPerson, Person>(importPerson, updatedPerson));
			if (currentPerson.VersionDate == versionDate)
			{
				continue;
			}
			await UpdateCurrentPersonDetailsAsync(currentPerson, importPerson, versionDate, source);
		}
		return updatedPersons;
	}

	private async Task UpdateCurrentPersonDetailsAsync(Person currentPerson, ImportPerson importPerson, DateTime versionDate, OriginSourceType source)
	{
		await UpdatePersonBaseRecordAsync(currentPerson, importPerson, versionDate, source);
		await UpdatePersonExternDatabaseIdDataAsync(currentPerson, importPerson.PersonExternDatabaseIds, versionDate, source);
		await UpdatePersonNameDataAsync(currentPerson, importPerson.PersonNames, versionDate, source);
	}

	private async Task UpdatePersonBaseRecordAsync(
		Person currentPerson,
		ImportPerson importPerson,
		DateTime versionDate,
		OriginSourceType source)
	{
		if (currentPerson.VersionDate < versionDate)
		{
			Person personForUpdate = _mapper.Map<Person>(importPerson);
			personForUpdate.Id = currentPerson.Id;
			await UpdateRecordAsync<Person, UpdatePersonCommand>(
				personForUpdate,
				versionDate,
				source);
		}
	}

	private async Task UpdatePersonExternDatabaseIdDataAsync(
		Person currentPerson,
		IEnumerable<ImportPersonExternDatabaseId> importExternIdentifiers,
		DateTime versionDate,
		OriginSourceType source)
	{
		GetPersonExternDatabaseIdsQuery request = new() { PersonId = currentPerson.Id };
		IEnumerable<PersonExternDatabaseId> personCurrentExternIds = (await _mediator.Send(request))
			.PersonExternDatabaseIds;

		IEnumerable<ImportPersonExternDatabaseId> importExternIdToInsert = GetEPCObjectExternDatabaseIdsForInsertAsync(
																					importExternIdentifiers,
																					personCurrentExternIds);

		List<PersonExternDatabaseId> identifiersToInsert = _mapper.Map<List<PersonExternDatabaseId>>(importExternIdToInsert);

		foreach (PersonExternDatabaseId externIdentifier in identifiersToInsert)
		{
			externIdentifier.PersonId = currentPerson.Id;
		}

		await InsertRecordsAsync<PersonExternDatabaseId, CreatePersonExternDatabaseIdCommand>(
			identifiersToInsert,
			versionDate,
			source);

		IEnumerable<Tuple<ImportPersonExternDatabaseId, PersonExternDatabaseId>> recordTuplesForUpdate = GetEPCObjectExternDatabaseIdsForUpdateAsync(
			importExternIdentifiers,
			personCurrentExternIds,
			versionDate);

		foreach (Tuple<ImportPersonExternDatabaseId, PersonExternDatabaseId> tuple in recordTuplesForUpdate)
		{
			await UpdatePersonExternDatabaseIdAsync(tuple.Item1, tuple.Item2, versionDate, source);
		}
	}

	private async Task UpdatePersonExternDatabaseIdAsync(
		ImportPersonExternDatabaseId importRecord,
		PersonExternDatabaseId currRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		PersonExternDatabaseId recordForUpdate = _mapper.Map<PersonExternDatabaseId>(importRecord);
		recordForUpdate.PersonId = currRecord.Id;
		await UpdateRecordAsync<PersonExternDatabaseId, UpdatePersonExternDatabaseIdCommand>(
			  recordForUpdate,
			  versionDate,
			  source);
	}

	private async Task UpdatePersonNameDataAsync(
	Person currentPerson,
	IEnumerable<ImportPersonName> importPersonNames,
	DateTime versionDate,
	OriginSourceType source)
	{
		GetPersonNamesQuery request = new() { PersonId = currentPerson.Id };
		IEnumerable<PersonName> personCurrentNames = (await _mediator.Send(request)).PersonNames;

		IEnumerable<ImportPersonName> namesToInsert = from personImpName in importPersonNames
													  where !(from personCurrName in personCurrentNames
															  where personCurrName.FirstName == personImpName.FirstName &&
																	personCurrName.LastName == personImpName.LastName &&
																	personCurrName.NameType == personImpName.NameType
															  select 1).Any()
													  select personImpName;

		List<PersonName> mappedNamesToInsert = _mapper.Map<List<PersonName>>(namesToInsert);
		foreach (PersonName personName in mappedNamesToInsert)
		{
			personName.PersonId = currentPerson.Id;
		}

		await InsertRecordsAsync<PersonName, CreatePersonNameCommand>(mappedNamesToInsert, versionDate, source);

		IEnumerable<Tuple<ImportPersonName, PersonName>> nameTuplesToUpdate = from personCurrName in personCurrentNames
																			  join personImpName in importPersonNames on
																			  new
																			  {
																				  FirstName = personCurrName.FirstName,
																				  LastName = personCurrName.LastName,
																				  NameType = personCurrName.NameType
																			  }
																			  equals
																			  new
																			  {
																				  FirstName = personImpName.FirstName,
																				  LastName = personImpName.LastName,
																				  NameType = personImpName.NameType
																			  }
																			  where personCurrName.VersionDate < versionDate
																			  select new Tuple<ImportPersonName, PersonName>(
																				  personImpName,
																				  personCurrName);

		foreach (Tuple<ImportPersonName, PersonName> tuple in nameTuplesToUpdate)
		{
			await UpdatePersonNameAsync(tuple.Item1, tuple.Item2, versionDate, source);
		}
	}

	private async Task UpdatePersonNameAsync(
		ImportPersonName importRecord,
		PersonName currRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		PersonName recordForUpdate = _mapper.Map<PersonName>(importRecord);
		recordForUpdate.PersonId = currRecord.Id;
		await UpdateRecordAsync<PersonName, UpdatePersonNameCommand>(
				recordForUpdate,
				versionDate,
				source);
	}

	private async Task<IEnumerable<Tuple<ImportPerson, Person>>> GetOrCreatePublicationPersonImportDomainTuplesAsync(
		IEnumerable<ImportPerson> relatedPersons,
		DateTime recordVersion,
		OriginSourceType source)
	{
		List<Tuple<ImportPerson, Person>> publicationPersonImportDomainTuples = new();
		foreach (ImportPerson importPerson in relatedPersons)
		{
			Person? domainPerson = await FindOrCreatePersonAsync(importPerson, recordVersion, source);
			publicationPersonImportDomainTuples.Add(new Tuple<ImportPerson, Person>(importPerson, domainPerson));
		}

		return publicationPersonImportDomainTuples;
	}

	private async Task<Person> FindOrCreatePersonAsync(ImportPerson importPerson, DateTime recordVersion, OriginSourceType source)
	{
		Person? resultModel = null;
		long personId = -1;

		List<string> personExternDatabaseIds = importPerson.PersonExternDatabaseIds.Select(x => x.ExternIdentifierValue).ToList();
		GetAllPersonsExternDbIdsInSetQueryResponse foundPersonExternIdentifiers = await _mediator.Send(new GetAllPersonsExternDbIdsInSetQuery()
		{
			SearchedExternIdentifiers = personExternDatabaseIds
		});

		if (foundPersonExternIdentifiers.ExternDatabaseIds != null &&
			foundPersonExternIdentifiers.ExternDatabaseIds.Any())
		{
			personId = foundPersonExternIdentifiers.ExternDatabaseIds.First().Id;
			resultModel = await GetPersonByIdAsync(personId);
			if (resultModel != null)
			{
				return resultModel;
			}
		}

		return await CreatePersonAsync(importPerson, recordVersion, source);
	}

	private async Task<Person?> GetPersonByIdAsync(long id)
	{
		GetPersonQueryResponse? personResponse = await _mediator.Send(new GetPersonQuery() { PersonId = id });
		return personResponse.Person;
	}

	private async Task<Person> CreatePersonAsync(ImportPerson importPerson, DateTime versionDate, OriginSourceType source)
	{
		CreatePersonCommand createCommand = _mapper.Map<CreatePersonCommand>(importPerson);
		createCommand.VersionDate = versionDate;
		createCommand.OriginSourceType = source;

		return (await _mediator.Send(createCommand)).Person;
	}
}
