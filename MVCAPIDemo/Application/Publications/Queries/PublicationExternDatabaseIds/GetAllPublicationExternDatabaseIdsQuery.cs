using MediatR;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetAllPublicationExternDatabaseIdsQuery : IRequest<GetAllPublicationExternDatabaseIdsQueryResponse>
{
	public long PublicationId { get; set; }
}
