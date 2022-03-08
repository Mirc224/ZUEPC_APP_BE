using MediatR;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetPublicationPublicationIdentifiersQuery : IRequest<GetPublicationPublicationIdentifiersQueryResponse>
{
	public long PublicationId { get; set; }
}
