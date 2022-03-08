using MediatR;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetPublicationPublicationExternDatabaseIdsQuery : IRequest<GetPublicationPublicationExternDatabaseIdsQueryResponse>
{
	public long PublicationId { get; set; }
}
