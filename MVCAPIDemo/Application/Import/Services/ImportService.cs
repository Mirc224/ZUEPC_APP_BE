using AutoMapper;
using MediatR;
using System.Xml.Linq;
using ZUEPC.Application.Import.Commands;
using ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Commands.InstitutionNames;
using ZUEPC.Application.Institutions.Commands.Institutions;
using ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.Application.Institutions.Queries.Institutions;
using ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Commands.PersonNames;
using ZUEPC.Application.Persons.Commands.Persons;
using ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Queries.PersonNames;
using ZUEPC.Application.Persons.Queries.Persons;
using ZUEPC.Application.PublicationAuthors.Commands;
using ZUEPC.Application.PublicationAuthors.Queries;
using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.Application.RelatedPublications.Queries;
using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;
using ZUEPC.EvidencePublication.Base.Domain.Persons;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;
using ZUEPC.Import.Models;
using ZUEPC.Import.Models.Commond;
using ZUEPC.Import.Parser;
using static ZUEPC.Import.Models.ImportInstitution;
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

		await ProcessImportedPublication(importedPublication, versionDate, source);
	}

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

		return updatedPublication;
	}

	private async Task ProcessRelatedPublicationsAsync(
		Publication updatedPublication, 
		IEnumerable<ImportRelatedPublication> relatedPublications, 
		DateTime versionDate, 
		OriginSourceType source)
	{
		GetPublicationRelatedPublicationsCommandResponse response = await _mediator.Send(
			new GetPublicationRelatedPublicationsCommand()
			{
				SourcePublicationId = updatedPublication.Id
			});
		foreach(ImportRelatedPublication relatedPublication in relatedPublications)
		{
			await ProcessRelatedPublicationAsync(updatedPublication, relatedPublication, versionDate, source);
		}
	}

	private async Task ProcessRelatedPublicationAsync(Publication sourcePublication, ImportRelatedPublication importRelatedPublication, DateTime versionDate, OriginSourceType source)
	{
		if(importRelatedPublication.RelatedPublication is null)
		{
			return;
		}
		ImportPublication importPublication = importRelatedPublication.RelatedPublication;
		Publication destPublication = await ProcessImportedPublication(importPublication, versionDate, source);



		RelatedPublication relatedPublication = _mapper.Map<RelatedPublication>(importRelatedPublication);
		relatedPublication.PublicationId = sourcePublication.Id;
		relatedPublication.RelatedPublicationId = destPublication.Id;

	}

	private async Task ProcessImportPublicationAuthorsAsync(
		Publication updatedPublication,
		IEnumerable<ImportPublicationAuthor> importPublicationAuthors, 
		DateTime versionDate, 
		OriginSourceType source)
	{
		IEnumerable<ImportPerson> relatedPersons = importPublicationAuthors.Select(x => x.Person).ToList();
		

		IEnumerable<Tuple<ImportPerson, Person>> updatedCurrentPersonsTuple = await ProcessImportPersonsAsync(
			relatedPersons,
			versionDate,
			source);

		IEnumerable<ImportInstitution> relatedInstitutions = importPublicationAuthors
														.Select(x => x.ReportingInstitution)
														.ToList();


		IEnumerable<Tuple<ImportInstitution, Institution>> updatedCurrentInstitutions = await ProcessImportedInstitutions(
																								relatedInstitutions,
																								versionDate,
																								source);

		IEnumerable<PublicationAuthor> foundPublicationAuthors = (await _mediator.Send(new GetAllPublicationAuthorsQuery()
		{
			PublicationId = updatedPublication.Id
		})).PublicationAuthors;

		IEnumerable<Tuple<ImportPublicationAuthor, Publication, Person, Institution>> authorsTuples = GetPublicationAuthorTuples(
																												updatedPublication,
																												importPublicationAuthors,
																												updatedCurrentPersonsTuple,
																												updatedCurrentInstitutions);

		IEnumerable<Tuple<ImportPublicationAuthor, PublicationAuthor>> publicationAuthorTuples =
			await GetCreatedOrUpdatedPublicationAuthorImportDomainTuplesAsync(authorsTuples,
																		  foundPublicationAuthors,
																		  versionDate,
																		  source);
	}

	private async Task<IEnumerable<Tuple<ImportInstitution, Institution>>> ProcessImportedInstitutions(IEnumerable<ImportInstitution> relatedInstitutions, DateTime versionDate, OriginSourceType source)
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

	private IEnumerable<Tuple<ImportPublicationAuthor, Publication, Person, Institution>> GetPublicationAuthorTuples(
		Publication updatedPublication,
		IEnumerable<ImportPublicationAuthor> importPublicationAuthors,
		IEnumerable<Tuple<ImportPerson, Person>> updatedCurrentPersonsTuple,
		IEnumerable<Tuple<ImportInstitution, Institution>> updatedCurrentInstitutions)
	{
		IEnumerable<Tuple<ImportPublicationAuthor, Publication, Person, Institution>> authorsTuples =
							from publicationAuthor in importPublicationAuthors
							join currPersonTuple in updatedCurrentPersonsTuple
							on publicationAuthor.Person equals currPersonTuple.Item1
							select new
							{
								Author = currPersonTuple.Item2,
								Institution = publicationAuthor.ReportingInstitution,
								ImportAuthor = publicationAuthor
							} into intermediate
							join publicationInstitutionTuple in updatedCurrentInstitutions
							on intermediate.Institution equals publicationInstitutionTuple.Item1
							select new Tuple<ImportPublicationAuthor, Publication, Person, Institution>(
																			   intermediate.ImportAuthor,
																			   updatedPublication,
																			   intermediate.Author,
																			   publicationInstitutionTuple.Item2);
		return authorsTuples;
	}

	private async Task<IEnumerable<Tuple<ImportPublicationAuthor, PublicationAuthor>>> GetCreatedOrUpdatedPublicationAuthorImportDomainTuplesAsync(
		IEnumerable<Tuple<ImportPublicationAuthor, Publication, Person, Institution>> authorsTuples,
		IEnumerable<PublicationAuthor> foundPublicationAuthors,
		DateTime versionDate,
		OriginSourceType source)
	{
		if (!authorsTuples.Any())
		{
			return new List<Tuple<ImportPublicationAuthor, PublicationAuthor>>();
		}

		IEnumerable<Tuple<ImportPublicationAuthor, Publication, Person, Institution>>
			authorsToInsert = from authorTuple in authorsTuples
							  where !(from foundAuthor in foundPublicationAuthors
									  where
									  foundAuthor.PublicationId == authorTuple.Item2.Id &&
									  foundAuthor.PersonId == authorTuple.Item3.Id &&
									  foundAuthor.InstitutionId == authorTuple.Item4.Id
									  select 1).Any()
							  select authorTuple;

		List<Tuple<ImportPublicationAuthor, PublicationAuthor>> publicationAuthorImportDomainTuples = new();
		foreach (Tuple<ImportPublicationAuthor, Publication, Person, Institution> tupleToInsert in authorsToInsert)
		{
			PublicationAuthor newPublicationAuthor = await CreatePublicationAuthorAsync(tupleToInsert, versionDate, source);
			publicationAuthorImportDomainTuples.Add(
				new Tuple<ImportPublicationAuthor, PublicationAuthor>(tupleToInsert.Item1, newPublicationAuthor));
		}

		IEnumerable<Tuple<ImportPublicationAuthor, PublicationAuthor, Publication, Person, Institution>>
			authorsToUpdate = from authorTuple in authorsTuples
							  join foundPublicationAuthor in foundPublicationAuthors on
							  new
							  {
								  PublicationId = authorTuple.Item2.Id,
								  PersonId = authorTuple.Item3.Id,
								  InstitutionId = authorTuple.Item4.Id
							  }
							  equals
							  new
							  {
								  PublicationId = foundPublicationAuthor.PublicationId,
								  PersonId = foundPublicationAuthor.PersonId,
								  InstitutionId = foundPublicationAuthor.InstitutionId
							  }
							  where foundPublicationAuthor.VersionDate < versionDate
							  select new Tuple<ImportPublicationAuthor, PublicationAuthor, Publication, Person, Institution>(
								  authorTuple.Item1,
								  foundPublicationAuthor,
								  authorTuple.Item2,
								  authorTuple.Item3,
								  authorTuple.Item4);

		foreach (var tupleToUpdate in authorsToUpdate)
		{
			PublicationAuthor newPublicationAuthor = await UpdatePublicationAuthorAsync(tupleToUpdate, versionDate, source);
			publicationAuthorImportDomainTuples.Add(
				new Tuple<ImportPublicationAuthor, PublicationAuthor>(tupleToUpdate.Item1, newPublicationAuthor));
		}

		return publicationAuthorImportDomainTuples;
	}

	private async Task<PublicationAuthor> UpdatePublicationAuthorAsync(
		Tuple<ImportPublicationAuthor, PublicationAuthor, Publication, Person, Institution> tupleToUpdate, 
		DateTime versionDate, 
		OriginSourceType source)
	{

		ImportPublicationAuthor importAuthor = tupleToUpdate.Item1;
		PublicationAuthor publicationAuthor = tupleToUpdate.Item2;
		Publication publication = tupleToUpdate.Item3;
		Person authorPerson = tupleToUpdate.Item4;
		Institution reportingInstitution = tupleToUpdate.Item5;

		PublicationAuthor updatedAuthor = _mapper.Map<PublicationAuthor>(importAuthor);
		updatedAuthor.VersionDate = versionDate;
		updatedAuthor.OriginSourceType = source;
		updatedAuthor.PublicationId = publication.Id;
		updatedAuthor.PersonId = authorPerson.Id;
		updatedAuthor.InstitutionId = reportingInstitution.Id;
		updatedAuthor.Id = publicationAuthor.Id;

		if(publicationAuthor.VersionDate < versionDate)
			await UpdateRecordAsync<PublicationAuthor, UpdatePublicationAuthorCommand>(updatedAuthor, versionDate, source);

		return updatedAuthor;
	}

	private async Task<PublicationAuthor> CreatePublicationAuthorAsync(
		Tuple<ImportPublicationAuthor, Publication, Person, Institution> tupleToInsert,
		DateTime versionDate,
		OriginSourceType source
		)
	{
		Publication publication = tupleToInsert.Item2;
		ImportPublicationAuthor importAuthor = tupleToInsert.Item1;
		Person authorPerson = tupleToInsert.Item3;
		Institution reportingInstitution = tupleToInsert.Item4;
		
		PublicationAuthor publicationAuthor = _mapper.Map<PublicationAuthor>(importAuthor);
		publicationAuthor.VersionDate = versionDate;
		publicationAuthor.OriginSourceType = source;
		publicationAuthor.PublicationId = publication.Id;
		publicationAuthor.PersonId = authorPerson.Id;
		publicationAuthor.InstitutionId = reportingInstitution.Id;

		CreatePublicationAuthorCommand request = _mapper.Map<CreatePublicationAuthorCommand>(publicationAuthor);
		return (await _mediator.Send(request)).CreatedPublicationAuthor;
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
			if(versionDate == currentInstitution.VersionDate)
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

	private async Task UpdateInstitutionNamesAsync(
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
		List<InstitutionName> recordsForUpdate = new();
		foreach (Tuple<ImportInstitutionName, InstitutionName> nameTuple in nameTuplesToUpdate)
		{
			ImportInstitutionName import = nameTuple.Item1;
			InstitutionName current = nameTuple.Item2;
			InstitutionName recordForUpdate = _mapper.Map<InstitutionName>(import);
			recordForUpdate.InstitutionId = current.Id;
			recordsForUpdate.Add(recordForUpdate);
		}

		if (recordsForUpdate.Any())
		{
			await UpdateRecordsAsync<InstitutionName, UpdateInstitutionNameCommand>(
			  recordsForUpdate,
			  versionDate,
			  source);
		}
	}

	private async Task UpdateInstitutionExternDatabaseIdsAsync(
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

		List<InstitutionExternDatabaseId> recordsForUpdate = new();
		foreach (Tuple<ImportInstitutionExternDatabaseId, InstitutionExternDatabaseId> tuple in recordTuplesForUpdate)
		{
			ImportInstitutionExternDatabaseId import = tuple.Item1;
			InstitutionExternDatabaseId current = tuple.Item2;
			InstitutionExternDatabaseId recordForUpdate = _mapper.Map<InstitutionExternDatabaseId>(import);
			recordForUpdate.InstitutionId = current.Id;
			recordsForUpdate.Add(recordForUpdate);
		}

		if (recordsForUpdate.Any())
		{
			await UpdateRecordsAsync<InstitutionExternDatabaseId, UpdateInstitutionExternDatabaseIdCommand>(
			  recordsForUpdate,
			  versionDate,
			  source);
		}
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

	private async Task<IEnumerable<Tuple<ImportPerson,Person>>> UpdateCurrentPersonsDetailsAsync(
		IEnumerable<Tuple<ImportPerson, Person>> publicationPersonsTuples,
		DateTime versionDate,
		OriginSourceType source)
	{
		List<Tuple<ImportPerson,Person>> updatedPersons = new();
		foreach (Tuple<ImportPerson, Person> personTuple in publicationPersonsTuples)
		{
			ImportPerson importPerson = personTuple.Item1;
			Person currentPerson = personTuple.Item2;
			Person updatedPerson = _mapper.Map<Person>(importPerson);
			updatedPerson.Id = currentPerson.Id;

			updatedPersons.Add(new Tuple<ImportPerson, Person>(importPerson, updatedPerson));
			if(currentPerson.VersionDate == versionDate)
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

		await InsertRecordsAsync<PersonExternDatabaseId, CreatePersonExternDatabaseIdCommand>(
			identifiersToInsert, 
			versionDate, 
			source);

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

	private async Task<Publication> UpdateCurrentPublicationDetailsAsync(
		Publication currentPublication,
		ImportPublication importPublication,
		DateTime versionDate,
		OriginSourceType source)
	{
		Publication updatedPublication = _mapper.Map<Publication>(importPublication);
		updatedPublication.Id = currentPublication.Id;
		await UpdatePublicationBaseRecordAsync(currentPublication, importPublication, versionDate, source);
		await UpdatePublicationIdentifiersAsync(currentPublication, importPublication.PublicationIdentifiers, versionDate, source);
		await UpdatePublicationExternDatabaseIdsAsync(currentPublication, importPublication.PublicationExternDbIds, versionDate, source);
		await UpdatePublicationNamesAsync(currentPublication, importPublication.PublicationNames, versionDate, source);
		return updatedPublication;
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
																						 new { 
																							 Name = publicationCurrName.Name, 
																							 NameType = publicationCurrName.NameType 
																							 }
																						 equals
																						 new { 
																							 Name = publicationImpName.Name, 
																							 NameType = publicationImpName.NameType 
																							 }
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
}
