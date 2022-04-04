using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetPublicationExternDatabaseIdQuery : 
	EPCQueryWithIdBase<long>,
	IRequest<GetPublicationExternDatabaseIdQueryResponse>
{
}
