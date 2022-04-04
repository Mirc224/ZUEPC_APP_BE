using MediatR;
using ZUEPC.Base.Queries;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetUserModelByIdQuery:
	EPCQueryWithIdBase<long>,
	IRequest<GetUserModelByIdQueryResponse>
{
}
