using MediatR;

namespace ZUEPC.Application.Publications.Queries.Publictions;

public class GetPublicationQuery : IRequest<GetPublicationQueryResponse>
{
	public long PublicationId { get; set; }
}
