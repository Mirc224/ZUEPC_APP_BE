using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Publications.Queries.Publications;
using ZUEPC.Api.Application.Publications.Queries.Publications.Details;
using ZUEPC.Api.Application.RelatedPublications.Queries;
using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.RelatedPublications.Entities.Details;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Domain.Publications;
using ZUEPC.EvidencePublication.Domain.RelatedPublications;

namespace ZUEPC.Application.Publications.Queries.Publications.Details.BaseHandlers;

public class EPCPublicationDetailsQueryHandlerBase
{
	protected readonly IMapper _mapper;
	protected readonly IMediator _mediator;

	public EPCPublicationDetailsQueryHandlerBase(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	protected async Task<PublicationDetails> ProcessPublicationDetails(Publication publicationDomain)
	{
		return (await ProcessPublicationPreviews(new Publication[] {publicationDomain})).First();
	}

	protected async Task<IEnumerable<PublicationDetails>> ProcessPublicationPreviews(IEnumerable<Publication> publicationDomains)
	{
		IEnumerable<PublicationDetails> result = _mapper.Map<List<PublicationDetails>>(publicationDomains);
		IEnumerable<long> publicationIds = result.Select(x => x.Id);

		IEnumerable<RelatedPublication> relatedPublications = (await _mediator
			.Send(new GetAllRelatedPublicationByPublicationIdInSetQuery() { PublicationIds = publicationIds }))
			.Data
			.OrEmptyIfNull();

		IEnumerable<long> relatedPublicationIds = relatedPublications.Select(x => x.RelatedPublicationId);
		IEnumerable<Publication> alreadyFoundRelatedPublicationDomains = publicationDomains
			.Where(x => relatedPublicationIds.Contains(x.Id))
			.ToList();
		IEnumerable<long> foundRelatedPublicationIds = alreadyFoundRelatedPublicationDomains.Select(x => x.Id).ToList();


		IEnumerable<long> relatedPublicationToGetIds = relatedPublicationIds.Where(x => !foundRelatedPublicationIds.Contains(x));
		IEnumerable<Publication> relaltedPublicationFromDbDomains = (await _mediator
			.Send(new GetAllPublicationsWithIdInSetQuery() { PublicationIds = relatedPublicationToGetIds }))
			.Data
			.OrEmptyIfNull();
		relaltedPublicationFromDbDomains = relaltedPublicationFromDbDomains.Concat(alreadyFoundRelatedPublicationDomains);

		IEnumerable<long> allPublicationIds = relatedPublicationToGetIds.Concat(publicationIds);

		GetAllPublicationsDetailsDataForPublicationIdsInSetQueryResponse previewData = await _mediator
			.Send(new GetAllPublicationsDetailsDataForPublicationIdsInSetQuery() { PublicationIds = allPublicationIds });

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

		IEnumerable<IGrouping<long, PublicationAuthorDetails>> authorDetailsGroupByPublicationId = previewData
			.PublicationAuthors
			.GroupBy(x => x.PublicationId);

		List<PublicationPreview> relatedPublicationPreviews = _mapper.Map<List<PublicationPreview>>(relaltedPublicationFromDbDomains);
		foreach (PublicationPreview relatedPubPrev in relatedPublicationPreviews)
		{
			relatedPubPrev.Names = namesGroupByPublicationId.Where(x => x.Key == relatedPubPrev.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			relatedPubPrev.Identifiers = identifiersGroupByPublicationId.Where(x => x.Key == relatedPubPrev.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			relatedPubPrev.ExternDatabaseIds = externDbIdsGroupByPublicationId.Where(x => x.Key == relatedPubPrev.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			relatedPubPrev.PublicationActivities = activitiesGroupByPublicationId.Where(x => x.Key == relatedPubPrev.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			relatedPubPrev.Authors = authorDetailsGroupByPublicationId.Where(x => x.Key == relatedPubPrev.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
		}

		List<RelatedPublicationDetails> relatedPublicationDetails = new();

		foreach (RelatedPublication relPub in relatedPublications)
		{
			RelatedPublicationDetails mappedRelPub = _mapper.Map<RelatedPublicationDetails>(relPub);
			mappedRelPub.RelatedPublication = relatedPublicationPreviews.Find(x => x.Id == relPub.RelatedPublicationId);
			relatedPublicationDetails.Add(mappedRelPub);
		}

		IEnumerable<IGrouping<long, RelatedPublicationDetails>> relatedPublicationDetailsGroupByPublicationId = relatedPublicationDetails
			.GroupBy(x => x.PublicationId);

		foreach (PublicationDetails publication in result)
		{
			publication.Names = namesGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.Identifiers = identifiersGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.ExternDatabaseIds = externDbIdsGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.PublicationActivities = activitiesGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.Authors = authorDetailsGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.RelatedPublications = relatedPublicationDetailsGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
		}
		return result;
	}
}
