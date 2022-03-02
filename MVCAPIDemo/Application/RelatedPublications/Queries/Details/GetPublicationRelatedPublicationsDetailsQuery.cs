using MediatR;

namespace ZUEPC.Application.RelatedPublications.Queries.Details;

public class GetPublicationRelatedPublicationsDetailsQuery : IRequest<GetPublicationRelatedPublicationsDetailsQueryResponse>
{
	public long SourcePublicationId { get; set; }
}
