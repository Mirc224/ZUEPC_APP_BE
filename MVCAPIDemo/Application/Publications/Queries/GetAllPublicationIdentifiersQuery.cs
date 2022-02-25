using MediatR;

namespace ZUEPC.Application.Publications.Queries;

public class GetAllPublicationIdentifiersQuery : IRequest<GetAllPublicationIdentifiersQueryResponse>
{
	public long PublicationId { get; set; }
}
