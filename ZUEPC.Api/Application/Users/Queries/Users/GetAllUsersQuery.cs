using MediatR;
using ZUEPC.Application.Users.Queries;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetAllUsersQuery:
	PaginationQueryWithUriBase,
	IRequest<GetAllUsersQueryResponse>
{
}
