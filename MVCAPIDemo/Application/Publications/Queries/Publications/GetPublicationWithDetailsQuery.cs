using MediatR;

namespace ZUEPC.Application.Publications.Queries.Publications;

public class GetPublicationWithDetailsQuery : IRequest<GetPublicationWithDetailsQueryResponse>
{
	public long PublicationId { get; set; }
}
