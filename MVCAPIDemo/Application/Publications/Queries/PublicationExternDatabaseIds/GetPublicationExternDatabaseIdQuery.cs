using MediatR;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetPublicationExternDatabaseIdQuery : IRequest<GetPublicationExternDatabaseIdQueryResponse>
{
	public long PublicationExternDatabaseIdId { get; set; }
}
