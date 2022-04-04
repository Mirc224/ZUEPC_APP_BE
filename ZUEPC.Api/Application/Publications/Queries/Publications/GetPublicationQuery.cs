using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.Publictions;

public class GetPublicationQuery : 
	EPCQueryWithIdBase<long>,
	IRequest<GetPublicationQueryResponse>
{
}
