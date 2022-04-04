using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetPublicationExternDatabaseIdQuery : 
	EPCSimpleQueryBase<long>,
	IRequest<GetPublicationExternDatabaseIdQueryResponse>
{
}
