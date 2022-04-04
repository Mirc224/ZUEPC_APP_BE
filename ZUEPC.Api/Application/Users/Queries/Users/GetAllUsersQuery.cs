using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetAllUsersQuery:
	PaginationWithFilterQueryBase<UserFilter>,
	IRequest<GetAllUsersQueryResponse>
{
}
