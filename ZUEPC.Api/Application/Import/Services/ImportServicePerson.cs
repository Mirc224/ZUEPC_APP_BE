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
	private async Task<IEnumerable<Tuple<ImportPerson, Person>>> ProcessImportPersonCollectionAsync(
		IEnumerable<ImportPerson> relatedPersons,
		DateTime versionDate,
		OriginSourceType source)
	{
		List<Tuple<ImportPerson, Person>> processedPersonsTuples = new();
		foreach (ImportPerson relatedPerson in relatedPersons)
		{
			Person updatedPerson = await ProcessImportPersonAsync(relatedPerson, versionDate, source);
			processedPersonsTuples.Add(new Tuple<ImportPerson, Person>(relatedPerson, updatedPerson));
		}

		return processedPersonsTuples;
	}


	private async Task<Person> ProcessImportPersonAsync(
		ImportPerson importPerson,
		DateTime versionDate,
		OriginSourceType source)
	{
		Person person = await FindOrCreatePersonAsync(importPerson, versionDate, source);
		if (person.VersionDate < versionDate)
		{
			person = await UpdatePersonBaseAsync(importPerson, person, versionDate, source);
		}
		await UpdatePersonExternDatabaseIdDataAsync(person, importPerson.PersonExternDatabaseIds, versionDate, source);
		await UpdatePersonNameDataAsync(person, importPerson.PersonNames, versionDate, source);

		return person;
	}

	private async Task<Person> UpdatePersonBaseAsync(ImportPerson importPerson, Person person, DateTime versionDate, OriginSourceType source)
	{
		Person updatedPerson = _mapper.Map<Person>(importPerson);
		updatedPerson.Id = person.Id;
		updatedPerson.VersionDate = versionDate;
		updatedPerson.OriginSourceType = source;
		await UpdateRecordAsync<Person, UpdatePersonCommand>(updatedPerson, versionDate, source);
		return updatedPerson;
	}

	private async Task UpdatePersonExternDatabaseIdDataAsync(
		Person currentPerson,
		IEnumerable<ImportPersonExternDatabaseId> importExternIdentifiers,
		DateTime versionDate,
		OriginSourceType source)
	{
		GetPersonPersonExternDatabaseIdsQuery request = new() { PersonId = currentPerson.Id };
		IEnumerable<PersonExternDatabaseId> personCurrentExternIds = (await _mediator.Send(request))
			.Data;

		IEnumerable<ImportPersonExternDatabaseId> importExternIdToInsert = GetEPCObjectExternDatabaseIdsForInsertAsync(
																					importExternIdentifiers,
																					personCurrentExternIds);

		await InsertPersonExternDatabaseIdCollectionAsync(currentPerson, importExternIdToInsert, versionDate, source);

		IEnumerable<Tuple<ImportPersonExternDatabaseId, PersonExternDatabaseId>> recordTuplesForUpdate = GetEPCObjectExternDatabaseIdsForUpdateAsync(
			importExternIdentifiers,
			personCurrentExternIds,
			versionDate);

		foreach (Tuple<ImportPersonExternDatabaseId, PersonExternDatabaseId> tuple in recordTuplesForUpdate)
		{
			await UpdatePersonExternDatabaseIdAsync(tuple.Item1, tuple.Item2, versionDate, source);
		}
	}

	private async Task InsertPersonExternDatabaseIdCollectionAsync(
		Person currentPerson,
		IEnumerable<ImportPersonExternDatabaseId> importPersonExternDbIds,
		DateTime versionDate,
		OriginSourceType source)
	{
		foreach (ImportPersonExternDatabaseId identifier in importPersonExternDbIds)
		{
			if (identifier.ExternIdentifierValue is null)
			{
				continue;
			}
			await InsertPersonExternDatabaseIdAsync(currentPerson, identifier, versionDate, source);
		}
	}

	public async Task InsertPersonExternDatabaseIdAsync(
		Person currentPerson,
		ImportPersonExternDatabaseId importPersonExternDbId,
		DateTime versionDate,
		OriginSourceType source)
	{
		PersonExternDatabaseId identifierToInsert = _mapper.Map<PersonExternDatabaseId>(importPersonExternDbId);
		identifierToInsert.PersonId = currentPerson.Id;
		await InsertRecordAsync<PersonExternDatabaseId, CreatePersonExternDatabaseIdCommand>(identifierToInsert, versionDate, source);
	}

	private async Task UpdatePersonExternDatabaseIdAsync(
		ImportPersonExternDatabaseId importRecord,
		PersonExternDatabaseId currRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		PersonExternDatabaseId recordForUpdate = _mapper.Map<PersonExternDatabaseId>(importRecord);
		recordForUpdate.Id = currRecord.Id;
		recordForUpdate.PersonId = currRecord.PersonId;
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
		GetPersonPersonNamesQuery request = new() { PersonId = currentPerson.Id };
		IEnumerable<PersonName> personCurrentNames = (await _mediator.Send(request)).Data;

		IEnumerable<ImportPersonName> namesToInsert = from personImpName in importPersonNames
													  where !(from personCurrName in personCurrentNames
															  where personCurrName.FirstName == personImpName.FirstName &&
																	personCurrName.LastName == personImpName.LastName &&
																	personCurrName.NameType == personImpName.NameType
															  select 1).Any() &&
															  (personImpName.FirstName != null ||
															   personImpName.LastName != null ||
															   personImpName.NameType != null)
													  select personImpName;

		await InsertPersonNameCollectionAsync(currentPerson, namesToInsert, versionDate, source);

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

	private async Task InsertPersonNameCollectionAsync(
		Person currentPerson,
		IEnumerable<ImportPersonName> importPersonNames,
		DateTime versionDate,
		OriginSourceType source)
	{
		foreach (ImportPersonName personName in importPersonNames)
		{
			if(personName.FirstName is null && personName.LastName is null)
			{
				continue;
			}
			await InsertPersonNameAsync(currentPerson, personName, versionDate, source);
		}
	}

	private async Task InsertPersonNameAsync(
		Person currentPerson,
		ImportPersonName importPersonNames,
		DateTime versionDate,
		OriginSourceType source)
	{
		PersonName nameToInsert = _mapper.Map<PersonName>(importPersonNames);
		nameToInsert.PersonId = currentPerson.Id;
		await InsertRecordAsync<PersonName, CreatePersonNameCommand>(nameToInsert, versionDate, source);
	}

	private async Task UpdatePersonNameAsync(
		ImportPersonName importRecord,
		PersonName currRecord,
		DateTime versionDate,
		OriginSourceType source)
	{
		PersonName recordForUpdate = _mapper.Map<PersonName>(importRecord);
		recordForUpdate.Id = currRecord.Id;
		recordForUpdate.PersonId = currRecord.PersonId;
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

		if (foundPersonExternIdentifiers.Data != null &&
			foundPersonExternIdentifiers.Data.Any())
		{
			personId = foundPersonExternIdentifiers.Data.First().PersonId;
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
		GetPersonQueryResponse? personResponse = await _mediator.Send(new GetPersonQuery() { Id = id });
		return personResponse.Data;
	}

	private async Task<Person> CreatePersonAsync(ImportPerson importPerson, DateTime versionDate, OriginSourceType source)
	{
		CreatePersonCommand createCommand = _mapper.Map<CreatePersonCommand>(importPerson);
		createCommand.VersionDate = versionDate;
		createCommand.OriginSourceType = source;
		Person newPerson = (await _mediator.Send(createCommand)).Data;
		await InsertPersonExternDatabaseIdCollectionAsync(newPerson, importPerson.PersonExternDatabaseIds, versionDate, source);
		await InsertPersonNameCollectionAsync(newPerson, importPerson.PersonNames, versionDate, source);
		return newPerson;
	}
}
