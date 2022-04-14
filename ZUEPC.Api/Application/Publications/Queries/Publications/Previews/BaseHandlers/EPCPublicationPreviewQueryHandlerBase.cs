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

		IEnumerable<IGrouping<long, PublicationAuthorDetails>> authorDetailsGroupByPublicationId = previewData
			.PublicationAuthors
			.GroupBy(x => x.PublicationId);


		foreach (PublicationPreview publication in result)
		{
			publication.Names = namesGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.Identifiers = identifiersGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.ExternDatabaseIds = externDbIdsGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.PublicationActivities = activitiesGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
			publication.Authors = authorDetailsGroupByPublicationId.Where(x => x.Key == publication.Id).Select(x => x.ToList()).FirstOrDefault().OrEmptyIfNull();
		}
		return result;
	}
}
