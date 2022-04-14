using MediatR;
using ZUEPC.Api.Application.PublicationActivities.Queries;
using ZUEPC.Api.Application.PublicationAuthors.Queries;
using ZUEPC.Api.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Api.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Api.Application.Publications.Queries.PublicationNames;
using ZUEPC.Base.Extensions;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Domain.Publications;
using ZUEPC.EvidencePublication.PublicationAuthors;

namespace ZUEPC.Api.Application.Publications.Queries.Publications.Previews;

public class GetAllPublicationsPreviewDataForPublicationIdsInSetQueryHandler :
	IRequestHandler<GetAllPublicationsPreviewDataForPublicationIdsInSetQuery, GetAllPublicationsPreviewDataForPublicationIdsInSetQueryResponse>
{
	private readonly IMediator _mediator;

	public GetAllPublicationsPreviewDataForPublicationIdsInSetQueryHandler(IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task<GetAllPublicationsPreviewDataForPublicationIdsInSetQueryResponse> Handle(GetAllPublicationsPreviewDataForPublicationIdsInSetQuery request, CancellationToken cancellationToken)
	{
		IEnumerable<long> publicationIds = request.PublicationIds.ToHashSet();
		return new() 
		{ 
			Success = true,
			PublicationNames = await GetPublicationNamesWithPublicationIdInSet(publicationIds),
			PublicationIdentifiers = await GetPublicationIdentifiersWithPublicationIdInSet(publicationIds),
			PublicationExternDbIds = await GetPublicationExternDbIdsWithPublicationIdInSet(publicationIds),
			PublicationActivities = await GetPublicationActivitiesWithPublicationIdInSet(publicationIds),
			PublicationAuthors = await GetPublicationAuthorsDetailsWithPublicationIdInSet(publicationIds)
		};
	}

	private async Task<IEnumerable<PublicationName>> GetPublicationNamesWithPublicationIdInSet(IEnumerable<long> publicationIds)
	{
		return (await _mediator.Send(new GetAllPublicationNamesByPublicationIdInSetQuery() { PublicationIds = publicationIds }))
			.Data
			.OrEmptyIfNull();
	}

	private async Task<IEnumerable<PublicationIdentifier>> GetPublicationIdentifiersWithPublicationIdInSet(IEnumerable<long> publicationIds)
	{
		return (await _mediator.Send(new GetAllPublicationIdentifiersByPublicationIdInSetQuery() { PublicationIds = publicationIds }))
			.Data
			.OrEmptyIfNull();
	}

	private async Task<IEnumerable<PublicationExternDatabaseId>> GetPublicationExternDbIdsWithPublicationIdInSet(IEnumerable<long> publicationIds)
	{
		return (await _mediator.Send(new GetAllPublicationExternDbIdsByPublicationIdInSetQuery() { PublicationIds = publicationIds }))
			.Data
			.OrEmptyIfNull();
	}

	private async Task<IEnumerable<PublicationActivity>> GetPublicationActivitiesWithPublicationIdInSet(IEnumerable<long> publicationIds)
	{
		return (await _mediator.Send(new GetAllPublicationActivitiesByPublicationIdInSetQuery() { PublicationIds = publicationIds }))
			.Data
			.OrEmptyIfNull();
	}

	private async Task<IEnumerable<PublicationAuthor>> GetPublicationAuthorsDetailsWithPublicationIdInSet(IEnumerable<long> publicationIds)
	{
		return (await _mediator.Send(new GetAllPublicationAuthorsByPublicationIdInSetQuery() { PublicationIds = publicationIds }))
			.Data
			.OrEmptyIfNull();
	}
}
