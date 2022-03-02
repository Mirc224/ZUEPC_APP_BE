using MediatR;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetPublicationIdentifiersQuery : IRequest<GetPublicationIdentifiersQueryResponse>
{
	public long PublicationId { get; set; }
}
