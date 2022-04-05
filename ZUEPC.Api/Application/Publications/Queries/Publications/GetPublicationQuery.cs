using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Application.Publications.Queries.Publictions;

public class GetPublicationQuery : 
	QueryWithIdBase<long>,
	IRequest<GetPublicationQueryResponse>
{
}
