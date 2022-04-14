using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Institutions.Queries.Institutions.Previews;
using ZUEPC.Api.Application.Persons.Queries.Persons.Previews;
using ZUEPC.Api.Application.Publications.Queries.Publications.Previews;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.PublicationActivities.Queries;
using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Application.PublicationAuthors.Queries.Details;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Domain.Publications;
using ZUEPC.EvidencePublication.PublicationAuthors;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews.BaseHandlers;

public abstract class EPCPublicationPreviewQueryHandlerBase
{
	protected readonly IMapper _mapper;
	protected readonly IMediator _mediator;

	public EPCPublicationPreviewQueryHandlerBase(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	protected async Task<PublicationPreview> ProcessPublicationPreview(Publication publicationDomain)
	{
		long publicationId = publicationDomain.Id;
		PublicationPreview resultPreview = _mapper.Map<PublicationPreview>(publicationDomain);
		resultPreview.Names = (await _mediator.Send(new GetPublicationPublicationNamesQuery()
		{
			PublicationId = publicationId
		})).Data;

		resultPreview.Identifiers = (await _mediator.Send(new GetPublicationPublicationIdentifiersQuery()
		{
			PublicationId = publicationId
		})).Data;

		resultPreview.Authors = (await _mediator.Send(new GetPublicationAuthorDetailsQuery()
		{
			PublicationId = publicationId
		})).Data;

		resultPreview.ExternDatabaseIds = (await _mediator.Send(new GetPublicationPublicationExternDatabaseIdsQuery()
		{
			PublicationId = publicationId
		})).Data;

		resultPreview.PublicationActivities = (await _mediator.Send(new GetPublicationPublicationActivitiesQuery()
		{
			PublicationId = publicationId
		})).Data;
		return resultPreview;
	}

	protected async Task<IEnumerable<PublicationPreview>> ProcessPublicationPreviews(IEnumerable<Publication> publicationDomains)
	{
		IEnumerable<PublicationPreview> result = _mapper.Map<List<PublicationPreview>>(publicationDomains);
		IEnumerable<long> publicationIds = result.Select(x => x.Id);

		GetAllPublicationsPreviewDataForPublicationIdsInSetQueryResponse previewData = await _mediator
			.Send(new GetAllPublicationsPreviewDataForPublicationIdsInSetQuery() { PublicationIds = publicationIds });

		IEnumerable<IGrouping<long, PublicationName>> namesGroupByPublicationId = previewData
			.PublicationNames
			.GroupBy(x => x.PublicationId);

		IEnumerable<IGrouping<long, PublicationIdentifier>> identifiersGroupByPublicationId = previewData
			.PublicationIdentifiers
			.GroupBy(x => x.PublicationId);

		IEnumerable<IGrouping<long, PublicationExternDatabaseId>> externDbIdsGroupByPublicationId = previewData
			.PublicationExternDbIds
			.GroupBy(x => x.PublicationId);

		IEnumerable<IGrouping<long, PublicationActivity>> activitiesGroupByPublicationId = previewData
			.PublicationActivities
			.GroupBy(x => x.PublicationId);

		IEnumerable<IGrouping<long, PublicationAuthor>> authorDetailsGroupByPublicationId = previewData
			.PublicationAuthors
			.GroupBy(x => x.PublicationId);

		IEnumerable<PersonPreview> personPreviews = await GetPersonPreviews(previewData
			.PublicationAuthors
			.Select(x => x.PersonId));
		IEnumerable<InstitutionPreview> institutionPreviews = await GetInstitutionPreviews(previewData
			.PublicationAuthors
			.Select(x => x.InstitutionId));

		foreach (PublicationPreview publication in result)
		{
			publication.Names = namesGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.Identifiers = identifiersGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.ExternDatabaseIds = externDbIdsGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.PublicationActivities = activitiesGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			List<PublicationAuthorDetails> authorsDetails = new();
			
			foreach (PublicationAuthor pubAuthor in authorDetailsGroupByPublicationId
				.Where(x => x.Key == publication.Id)
				.Select(x => x.ToList())
				.FirstOrDefault()
				.OrEmptyIfNull())
			{
				PublicationAuthorDetails authorDetails = _mapper.Map<PublicationAuthorDetails>(pubAuthor);
				authorDetails.PersonPreview = personPreviews.FirstOrDefault(x => x.Id == pubAuthor.PersonId);
				authorDetails.InstitutionPreview = institutionPreviews.FirstOrDefault(x => x.Id == pubAuthor.InstitutionId);
				authorsDetails.Add(authorDetails);
			}
			publication.Authors = authorsDetails;
		}
		return result;
	}

	private async Task<IEnumerable<PersonPreview>> GetPersonPreviews(IEnumerable<long> personIds)
	{
		return (await _mediator
			.Send(new GetAllPersonPreviewsForIdsInSetQuery() { PersonIds = personIds }))
			.Data
			.OrEmptyIfNull();
	}

	private async Task<IEnumerable<InstitutionPreview>> GetInstitutionPreviews(IEnumerable<long> institutionIds)
	{
		return (await _mediator
			.Send(new GetAllInstitutionsPreviewsForIdsInSetQuery() { InstitutionIds = institutionIds }))
			.Data
			.OrEmptyIfNull();
	}
}
