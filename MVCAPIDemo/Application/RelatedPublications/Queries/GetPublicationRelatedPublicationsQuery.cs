using MediatR;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetPublicationRelatedPublicationsQuery : IRequest<GetPublicationRelatedPublicationsQueryResponse>
{
	public long SourcePublicationId { get; set; }
}
