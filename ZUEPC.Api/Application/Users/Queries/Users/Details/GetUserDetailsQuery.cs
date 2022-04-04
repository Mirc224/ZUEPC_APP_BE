using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Api.Application.Users.Queries.Users.Details;

public class GetUserDetailsQuery :
	EPCQueryWithIdBase<long>,
	IRequest<GetUserDetailsQueryResponse>
{
}
