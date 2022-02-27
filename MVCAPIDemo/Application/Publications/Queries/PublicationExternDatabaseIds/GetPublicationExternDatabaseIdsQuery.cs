using MediatR;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetPublicationExternDatabaseIdsQuery : IRequest<GetPublicationExternDatabaseIdsQueryResponse>
{
	public long PublicationId { get; set; }
}
