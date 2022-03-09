using ZUEPC.Application.PublicationAuthors.Commands;
using ZUEPC.Application.PublicationAuthors.Queries;
using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;
using ZUEPC.EvidencePublication.Base.Domain.Persons;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;
using ZUEPC.Import.Models;

namespace ZUEPC.Application.Import.Services;

public partial class ImportService
{
	private async Task ProcessImportPublicationAuthorsAsync(
		Publication updatedPublication,
		IEnumerable<ImportPublicationAuthor> importPublicationAuthors,
		DateTime versionDate,
		OriginSourceType source)
	{
		IEnumerable<ImportPerson> relatedPersons = importPublicationAuthors.Select(x => x.Person).ToList();


		IEnumerable<Tuple<ImportPerson, Person>> updatedCurrentPersonsTuple = await ProcessImportPersonCollectionAsync(
			relatedPersons,
			versionDate,
			source);

		IEnumerable<ImportInstitution> relatedInstitutions = importPublicationAuthors
																.Select(x => x.ReportingInstitution)
																.ToList();


		IEnumerable<Tuple<ImportInstitution, Institution>> updatedCurrentInstitutions = await ProcessImportInstitutionsCollectionAsync(
																								relatedInstitutions,
																								versionDate,
																								source);

		IEnumerable<PublicationAuthor> foundPublicationAuthors = (await _mediator.Send(new GetPublicationPublicationAuthorsQuery()
		{
			PublicationId = updatedPublication.Id
		})).Data;

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
		updatedAuthor.Id = publicationAuthor.Id;
		updatedAuthor.PublicationId = publication.Id;
		updatedAuthor.PersonId = authorPerson.Id;
		updatedAuthor.InstitutionId = reportingInstitution.Id;
		updatedAuthor.VersionDate = versionDate;
		updatedAuthor.OriginSourceType = source;

		if (publicationAuthor.VersionDate < versionDate)
		{
			await UpdateRecordAsync<PublicationAuthor, UpdatePublicationAuthorCommand>(updatedAuthor, versionDate, source);
		}

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
		return (await _mediator.Send(request)).Data;
	}
}
