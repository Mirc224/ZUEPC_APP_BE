using AutoMapper;
using MediatR;
using ZUEPC.Application.PublicationActivities.Queries;
using ZUEPC.Application.PublicationAuthors.Queries.Details;
using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Application.RelatedPublications.Queries.Details;
using ZUEPC.EvidencePublication.Domain.Publications;

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
}
