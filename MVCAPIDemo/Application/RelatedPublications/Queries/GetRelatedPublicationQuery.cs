using MediatR;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetRelatedPublicationQuery : IRequest<GetRelatedPublicationQueryResponse>
{
	public long RelatedPublicationRecordId { get; set; }
}
