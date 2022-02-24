using MediatR;

namespace ZUEPC.Application.Publications.Queries;

public class GetPublicationQuery : IRequest<GetPublicationQueryResponse>
{
	public long PublicationId { get; set; }
}
