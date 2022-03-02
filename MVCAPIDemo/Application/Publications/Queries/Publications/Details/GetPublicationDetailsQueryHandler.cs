using AutoMapper;
using MediatR;
using ZUEPC.Application.PublicationActivities.Queries;
using ZUEPC.Application.PublicationAuthors.Queries.Details;
using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.Application.RelatedPublications.Queries.Details;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.Publications.Details;

public class GetPublicationDetailsQueryHandler : IRequestHandler<GetPublicationDetailsQuery, GetPublicationDetailsQueryResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public GetPublicationDetailsQueryHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}
	public async Task<GetPublicationDetailsQueryResponse> Handle(GetPublicationDetailsQuery request, CancellationToken cancellationToken)
	{
		long publicationId = request.PublicationId;
		Publication? publication = (await _mediator.Send(new GetPublicationQuery()
		{
			PublicationId = publicationId
		})).Publication;

		if (publication is null)
		{
			return new() { Success = false };
		}
		PublicationDetails result = _mapper.Map<PublicationDetails>(publication);
		result.Names = (await _mediator.Send(new GetPublicationNamesQuery()
		{
			PublicationId = publicationId
		})).PublicationNames;

		result.Identifiers = (await _mediator.Send(new GetPublicationIdentifiersQuery() 
		{ 
			PublicationId = publicationId 
		})).PublicationIdentifiers;

		result.ExternDatabaseIds = (await _mediator.Send(new GetPublicationExternDatabaseIdsQuery()
		{
			PublicationId = publicationId
		})).PublicationExternDatabaseIds;

		result.PublicationAuthors = (await _mediator.Send(new GetPublicationAuthorDetailsQuery()
		{
			PublicationId = publicationId
		})).PublicationAuthorDetails;

		result.RelatedPublications = (await _mediator.Send(new GetPublicationRelatedPublicationsDetailsQuery()
		{
			SourcePublicationId = publicationId
		})).RelatedPublications;

		result.PublicationActivities = (await _mediator.Send(new GetPublicationActivitiesQuery()
		{
			PublicationId = publicationId
		})).PublicationActivities;

		return new() { Success = true, PublicationWithDetails = result };
	}
}
