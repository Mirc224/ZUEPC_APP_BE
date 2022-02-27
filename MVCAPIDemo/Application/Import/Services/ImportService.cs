using AutoMapper;
using MediatR;
using System.Xml.Linq;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Application.Institutions.Commands.Institutions;
using ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Queries.Institutions;
using ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Commands.PersonNames;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Queries.PersonNames;
using ZUEPC.Application.Persons.Queries.Persons;
using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;
using ZUEPC.EvidencePublication.Base.Domain.Persons;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.Import.Models;
using ZUEPC.Import.Models.Commond;
using ZUEPC.Import.Parser;
using static ZUEPC.Import.Models.ImportPerson;
using static ZUEPC.Import.Models.ImportPublication;

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
		IEnumerable<ImportRecord>? result = ParseImportXMLCommand(command, OriginSourceType.CREPC);
		ProcessImportedRecords(result, OriginSourceType.CREPC);
	}

	public void ImportFromDaWinciXML(ImportDaWinciXmlCommand command)
	{
		IEnumerable<ImportRecord>? result = ParseImportXMLCommand(command, OriginSourceType.DAWINCI);
		ProcessImportedRecords(result, OriginSourceType.DAWINCI);
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

	private void ProcessImportedRecords(IEnumerable<ImportRecord>? records, OriginSourceType source)
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
			Task? task = ProccesImportedRecordAsync(item, source);
			task.Wait();
		}
	}

	private async Task ProccesImportedRecordAsync(ImportRecord record, OriginSourceType source)
	{
		ImportPublication importedPublication = record.Publication;
		DateTime versionDate = record.RecordVersionDate;

		Publication publication = await FindOrCreatePublication(importedPublication, versionDate, source);
		await UpdateCurrentPublicationDetailsAsync(publication, importedPublication, versionDate, source);
		List<ImportPerson> relatedPersons = importedPublication.PublicationAuthors.Select(x => x.Person).ToList();
		IEnumerable<Tuple<ImportPerson, Person>> publicationPersonsTuples = await GetOrCreatePublicationPersonImportDomainTuplesAsync(
																				  relatedPersons,
																				  versionDate,
																				  source);
		await UpdateCurrentPersonsDetailsAsync(publicationPersonsTuples, versionDate, source);

		List<ImportInstitution> relatedInstitutions = importedPublication.PublicationAuthors.Select(x => x.ReportingInstitution).ToList();
		IEnumerable<Tuple<ImportInstitution, Institution>> publicationInstitutionTuples = await GetOrCreatePublicationInstitutionImportDomainTuplesAsync(
																								relatedInstitutions,
																								versionDate,
																								source);
		await UpdateCurrentInstitutionsDetailsAsync(publicationInstitutionTuples, versionDate, source);
	}

	private async Task UpdateCurrentInstitutionsDetailsAsync(
		IEnumerable<Tuple<ImportInstitution, Institution>> publicationInstitutionTuples,
		DateTime versionDate,
		OriginSourceType source)
	{
		foreach (Tuple<ImportInstitution, Institution> institutionTuple in publicationInstitutionTuples)
		{
			ImportInstitution importInstitution = institutionTuple.Item1;
			Institution currentInstitution = institutionTuple.Item2;
			await UpdateCurrentInstitutionDetailsAsync(currentInstitution, importInstitution, versionDate, source);
		}
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
		await UpdateInstitutionExternDatabaseIdsAsync(
			currentInstitution,
			importInstitution.InstitutionExternDatabaseIds,
			versionDate,
			source);
		await UpdateInstitutionNamesAsync(
			currentInstitution,
			importInstitution.InstitutionNames,
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

	private async Task<IEnumerable<Tuple<ImportInstitution, Institution>>> GetOrCreatePublicationInstitutionImportDomainTuplesAsync(
		IEnumerable<ImportInstitution> relatedInstitutions, 
		DateTime versionDate, 
		OriginSourceType source)
	{
		List<Tuple<ImportInstitution, Institution>> publicationInstitutionImportDomainTuples = new();
		foreach (ImportInstitution importInstitution in relatedInstitutions)
		{
			Institution? domainInstitution = await FindOrCreateInstitution(importInstitution, versionDate, source);
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

	private async Task UpdateCurrentPersonsDetailsAsync(
		IEnumerable<Tuple<ImportPerson, Person>> publicationPersonsTuples,
		DateTime versionDate,
		OriginSourceType source)
	{
		foreach (Tuple<ImportPerson, Person> personTuple in publicationPersonsTuples)
		{
			ImportPerson importPerson = personTuple.Item1;
			Person currentPerson = personTuple.Item2;
			await UpdateCurrentPersonDetailsAsync(currentPerson, importPerson, versionDate, source);
		}
	}

	private async Task UpdateCurrentPersonDetailsAsync(Person currentPerson, ImportPerson importPerson, DateTime versionDate, OriginSourceType source)
	{
		await UpdatePersonBaseRecordAsync(currentPerson, importPerson, versionDate, source);
		await UpdatePersonExternDatabaseIdsAsync(currentPerson, importPerson.PersonExternDatabaseIds, versionDate, source);
		await UpdatePersonNamesAsync(currentPerson, importPerson.PersonNames, versionDate, source);
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

	private async Task UpdatePersonExternDatabaseIdsAsync(
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

		IEnumerable<Tuple<ImportPersonExternDatabaseId, PersonExternDatabaseId>> recordTuplesForUpdate = GetEPCObjectExternDatabaseIdsForUpdateAsync(
			importExternIdentifiers,
			personCurrentExternIds,
			versionDate);

		List<PersonExternDatabaseId> recordsForUpdate = new();
		foreach (Tuple<ImportPersonExternDatabaseId, PersonExternDatabaseId> tuple in recordTuplesForUpdate)
		{
			ImportPersonExternDatabaseId import = tuple.Item1;
			PersonExternDatabaseId current = tuple.Item2;
			PersonExternDatabaseId recordForUpdate = _mapper.Map<PersonExternDatabaseId>(import);
			recordForUpdate.PersonId = current.Id;
			recordsForUpdate.Add(recordForUpdate);
		}

		if (recordsForUpdate.Any())
		{
			await UpdateRecordsAsync<PersonExternDatabaseId, UpdatePersonExternDatabaseIdCommand>(
			  recordsForUpdate,
			  versionDate,
			  source);
		}
	}

	private async Task UpdatePersonNamesAsync(
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
																						new { 
																							FirstName = personCurrName.FirstName, 
																							LastName = personCurrName.LastName,
																							NameType = personCurrName.NameType 
																							}
																						equals
																						new {
																							FirstName = personImpName.FirstName,
																							LastName = personImpName.LastName,
																							NameType = personImpName.NameType 
																							}
																						where personCurrName.VersionDate < versionDate
																			  select new Tuple<ImportPersonName, PersonName>(
																				  personImpName, 
																				  personCurrName);

		List<PersonName> recordsForUpdate = new();
		foreach (Tuple<ImportPersonName, PersonName> nameTuple in nameTuplesToUpdate)
		{
			ImportPersonName import = nameTuple.Item1;
			PersonName current = nameTuple.Item2;
			PersonName recordForUpdate = _mapper.Map<PersonName>(import);
			recordForUpdate.PersonId = current.Id;
			recordsForUpdate.Add(recordForUpdate);
		}

		if (recordsForUpdate.Any())
		{
			await UpdateRecordsAsync<PersonName, UpdatePersonNameCommand>(
			  recordsForUpdate,
			  versionDate,
			  source);
		}
	}

	private async Task<IEnumerable<Tuple<ImportPerson, Person>>> GetOrCreatePublicationPersonImportDomainTuplesAsync(
		IEnumerable<ImportPerson> relatedPersons,
		DateTime recordVersion,
		OriginSourceType source)
	{
		List<Tuple<ImportPerson, Person>> publicationPersonImportDomainTuples = new();
		foreach (ImportPerson importPerson in relatedPersons)
		{
			Person? domainPerson = await FindOrCreatePerson(importPerson, recordVersion, source);
			publicationPersonImportDomainTuples.Add(new Tuple<ImportPerson, Person>(importPerson, domainPerson));
		}

		return publicationPersonImportDomainTuples;
	}

	private async Task<Person> FindOrCreatePerson(ImportPerson importPerson, DateTime recordVersion, OriginSourceType source)
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

	private async Task UpdateCurrentPublicationDetailsAsync(
		Publication currentPublication,
		ImportPublication importPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		await UpdatePublicationBaseRecordAsync(currentPublication, importPublication, versionDate, source);
		await UpdatePublicationIdentifiersAsync(currentPublication, importPublication.PublicationIdentifiers, versionDate, source);
		await UpdatePublicationExternDatabaseIdsAsync(currentPublication, importPublication.PublicationExternDbIds, versionDate, source);
		await UpdatePublicationNamesAsync(currentPublication, importPublication.PublicationNames, versionDate, source);
	}


	private async Task UpdatePublicationNamesAsync(
		Publication currentPublication,
		IEnumerable<ImportPublicationName> importPublicationNames,
		DateTime versionDate,
		OriginSourceType source)
	{
		GetPublicationNamesQuery request = new() { PublicationId = currentPublication.Id };
		IEnumerable<PublicationName> publicationCurrentNames = (await _mediator.Send(request)).PublicationNames;

		List<string> allImportedNamesString = importPublicationNames.Select(identifier => identifier.Name).ToList();
		List<string> allCurrentNamesString = publicationCurrentNames.Select(identifier => identifier.Name).ToList();

		//IEnumerable<PublicationName> namesToDelete = from publicationCurrName in publicationCurrentNames
		//											  where publicationCurrName.VersionDate < versionDate &&
		//											  !(from publicationImpName in importPublicationNames
		//												where publicationImpName.Name == publicationCurrName.Name &&
		//													  publicationImpName.NameType == publicationCurrName.NameType
		//												select 1).Any()
		//											  select publicationCurrName;
		//await DeleteRecordsAsync<PublicationName, DeletePublicationNameCommand>(namesToDelete);

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
																						 new { Name = publicationCurrName.Name, NameType = publicationCurrName.NameType }
																						 equals
																						 new { Name = publicationImpName.Name, NameType = publicationImpName.NameType }
																						 where publicationCurrName.VersionDate < versionDate
																						 select new Tuple<ImportPublicationName, PublicationName>(publicationImpName, publicationCurrName);
		List<PublicationName> recordsForUpdate = new();
		foreach (Tuple<ImportPublicationName, PublicationName> nameTuple in nameTuplesToUpdate)
		{
			ImportPublicationName import = nameTuple.Item1;
			PublicationName current = nameTuple.Item2;
			PublicationName recordForUpdate = _mapper.Map<PublicationName>(import);
			recordForUpdate.PublicationId = current.Id;
			recordsForUpdate.Add(recordForUpdate);
		}

		if(recordsForUpdate.Any())
		{
			await UpdateRecordsAsync<PublicationName, UpdatePublicationNameCommand>(
			  recordsForUpdate,
			  versionDate,
			  source);
		}
	}

	private async Task UpdatePublicationBaseRecordAsync(
		Publication currentPublication,
		ImportPublication importPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		if (currentPublication.VersionDate < versionDate)
		{
			Publication publicationForUpdate = _mapper.Map<Publication>(importPublication);
			publicationForUpdate.Id = currentPublication.Id;
			await UpdateRecordAsync<Publication, UpdatePublicationCommand>(
				publicationForUpdate,
				versionDate,
				source);
		}
	}

	private async Task UpdatePublicationIdentifiersAsync(
		Publication currentPublication,
		IEnumerable<ImportPublicationIdentifier> importIdentifiers,
		DateTime versionDate,
		OriginSourceType source)
	{

		GetAllPublicationIdentifiersQuery request = new() { PublicationId = currentPublication.Id };
		IEnumerable<PublicationIdentifier> publicationCurrentIdentifiers = (await _mediator.Send(request)).PublicationIdentifiers;

		List<string> allImportedIdentifiersString = importIdentifiers.Select(identifier => identifier.IdentifierValue).ToList();
		List<string?> allCurrentIdentifiersString = publicationCurrentIdentifiers.Select(identifier => identifier.IdentifierValue).ToList();

		IEnumerable<PublicationIdentifier>? identifiersToDelete = publicationCurrentIdentifiers.Where(x => x.VersionDate < versionDate &&
																		  !allImportedIdentifiersString.Contains(x.IdentifierValue));


		await DeleteRecordsAsync<PublicationIdentifier, DeletePublicationIdentifierCommand>(identifiersToDelete);

		IEnumerable<ImportPublicationIdentifier> importIdentifiersToInsert = importIdentifiers.Where(x => !allCurrentIdentifiersString.Contains(x.IdentifierValue));
		List<PublicationIdentifier> identifiersToInsert = _mapper.Map<List<PublicationIdentifier>>(importIdentifiersToInsert);
		foreach (PublicationIdentifier identifier in identifiersToInsert)
		{
			identifier.PublicationId = currentPublication.Id;
		}

		await InsertRecordsAsync<PublicationIdentifier, CreatePublicationCommand>(identifiersToInsert, versionDate, source);

		IEnumerable<Tuple<ImportPublicationIdentifier, PublicationIdentifier>> identifiersTupleToUpdate =
			from current in publicationCurrentIdentifiers
			join import in importIdentifiers on current.IdentifierValue equals import.IdentifierValue
			where current.VersionDate < versionDate
			select new Tuple<ImportPublicationIdentifier, PublicationIdentifier>(import, current);

		List<PublicationIdentifier> recordsForUpdate = new();
		foreach(Tuple<ImportPublicationIdentifier, PublicationIdentifier> identifierTuple in identifiersTupleToUpdate)
		{
			ImportPublicationIdentifier import = identifierTuple.Item1;
			PublicationIdentifier current = identifierTuple.Item2;
			PublicationIdentifier recordForUpdate = _mapper.Map<PublicationIdentifier>(import);
			recordForUpdate.PublicationId = current.Id;
			recordsForUpdate.Add(recordForUpdate);
		}

		if(recordsForUpdate.Any())
		{
			await UpdateRecordsAsync<PublicationIdentifier, UpdatePublicationIdentifierCommand>(
			  recordsForUpdate,
			  versionDate,
			  source);
		}
	}

	private async Task UpdatePublicationExternDatabaseIdsAsync(
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

		List<PublicationExternDatabaseId> recordsForUpdate = new();
		foreach (Tuple<ImportPublicationExternDatabaseId, PublicationExternDatabaseId> tuple in recordTuplesForUpdate)
		{
			ImportPublicationExternDatabaseId import = tuple.Item1;
			PublicationExternDatabaseId current = tuple.Item2;
			PublicationExternDatabaseId recordForUpdate = _mapper.Map<PublicationExternDatabaseId>(import);
			recordForUpdate.PublicationId = current.Id;
			recordsForUpdate.Add(recordForUpdate);
		}

		if (recordsForUpdate.Any())
		{
			await UpdateRecordsAsync<PublicationExternDatabaseId, UpdatePublicationExternDatabaseIdCommand>(
			  recordsForUpdate,
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


	private async Task<Publication> CreatePublication(ImportPublication publication, DateTime versionDate, OriginSourceType source)
	{
		CreatePublicationCommand createCommand = _mapper.Map<CreatePublicationCommand>(publication);
		createCommand.VersionDate = versionDate;
		createCommand.OriginSourceType = source;
		return (await _mediator.Send(createCommand)).CreatedPublication;
	}

	private async Task<Publication?> GetPublicationById(long id)
	{
		GetPublicationQueryResponse publicationResponse = await _mediator.Send(new GetPublicationQuery() { PublicationId = id });
		return publicationResponse.Publication;
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
			TCommand insertRequest = _mapper.Map<TCommand>(record);
			insertRequest.VersionDate = versionDate;
			insertRequest.OriginSourceType = source;
			await _mediator.Send(insertRequest);
		}
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

	private IEnumerable<TImport> GetEPCObjectExternDatabaseIdsForInsertAsync<TImport,TDomain>(
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

	//private async Task UpdateAndDeleteEPCObjectExternDatabaseIdsAsync<
	//	TImport,
	//	TDomain,
	//	TDeleteCommand,
	//	TUpdateCommand>(
	//	IEnumerable<TDomain> objectCurrentExternIds,
	//	IEnumerable<TImport> importExternIdentifiers,
	//	DateTime versionDate,
	//	OriginSourceType source)
	//	where TImport : EPCImportExternDatabaseIdBase
	//	where TDomain : EPCExternDatabaseIdBase
	//	where TDeleteCommand : EPCDeleteBaseCommand, new()
	//	where TUpdateCommand : EPCUpdateBaseCommand, new()
	//{
	//	List<string> allImportedExternIdsString = importExternIdentifiers.Select(identifier => identifier.ExternIdentifierValue).ToList();
	//	List<string> allCurrentExternIdsString = objectCurrentExternIds.Select(identifier => identifier.ExternIdentifierValue).ToList();

	//	//if (source != OriginSourceType.DAWINCI)
	//	//{
	//	//	IEnumerable<TDomain> identifiersToDelete = objectCurrentExternIds.Where(x => x.VersionDate < versionDate &&
	//	//															!allImportedExternIdsString.Contains(x.ExternIdentifierValue));
	//	//	await DeleteRecordsAsync<TDomain, TDeleteCommand>(identifiersToDelete);
	//	//}

	//	IEnumerable<Tuple<TImport, TDomain>> identifiersTupleToUpdate =
	//		from current in objectCurrentExternIds
	//		join import in importExternIdentifiers on current.ExternIdentifierValue equals import.ExternIdentifierValue
	//		where current.VersionDate < versionDate
	//		select new Tuple<TImport, TDomain>(import, current);

	//	await UpdateRecordsAsync<TImport, TDomain, TUpdateCommand>(
	//		  identifiersTupleToUpdate,
	//		  versionDate,
	//		  source);
	//}
}
