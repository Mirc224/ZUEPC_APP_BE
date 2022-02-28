using MediatR;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetPublicationRelatedPublicationsCommand : IRequest<GetPublicationRelatedPublicationsCommandResponse>
{
	public long SourcePublicationId { get; set; }
}
