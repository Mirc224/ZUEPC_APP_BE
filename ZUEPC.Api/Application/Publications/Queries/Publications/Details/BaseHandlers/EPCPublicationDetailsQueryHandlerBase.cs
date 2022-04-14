﻿using AutoMapper;
using MediatR;
using ZUEPC.Api.Application.Publications.Queries.Publications;
using ZUEPC.Api.Application.Publications.Queries.Publications.Details;
using ZUEPC.Api.Application.RelatedPublications.Queries;
using ZUEPC.Application.PublicationActivities.Queries;
using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Application.PublicationAuthors.Queries.Details;
using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Application.RelatedPublications.Entities.Details;
using ZUEPC.Application.RelatedPublications.Queries.Details;
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
		long publicationId = publicationDomain.Id;
		PublicationDetails result = _mapper.Map<PublicationDetails>(publicationDomain);
		result.Names = (await _mediator.Send(new GetPublicationPublicationNamesQuery()
		{
			PublicationId = publicationId
		})).Data;

		result.Identifiers = (await _mediator.Send(new GetPublicationPublicationIdentifiersQuery()
		{
			PublicationId = publicationId
		})).Data;

		result.ExternDatabaseIds = (await _mediator.Send(new GetPublicationPublicationExternDatabaseIdsQuery()
		{
			PublicationId = publicationId
		})).Data;

		result.Authors = (await _mediator.Send(new GetPublicationAuthorDetailsQuery()
		{
			PublicationId = publicationId
		})).Data;

		result.RelatedPublications = (await _mediator.Send(new GetPublicationRelatedPublicationsDetailsQuery()
		{
			SourcePublicationId = publicationId
		})).Data;

		result.PublicationActivities = (await _mediator.Send(new GetPublicationPublicationActivitiesQuery()
		{
			PublicationId = publicationId
		})).Data;
		return result;
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
		IEnumerable<Publication> relaltedPublicationDomains = (await _mediator
			.Send(new GetAllPublicationsWithIdInSetQuery() { PublicationIds = relatedPublicationIds }))
			.Data
			.OrEmptyIfNull();

		IEnumerable<long> allPublicationIds = relatedPublicationIds.Concat(publicationIds);

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

		List<PublicationPreview> relatedPublicationPreviews = _mapper.Map<List<PublicationPreview>>(relaltedPublicationDomains);
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
