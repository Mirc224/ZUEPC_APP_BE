using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetAllUsersQuery:
	PaginatedQueryWithFilterBase<UserFilter>,
	IRequest<GetAllUsersQueryResponse>
{
}
