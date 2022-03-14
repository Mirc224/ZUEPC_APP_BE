using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetAllUsersQuery:
	PaginationWithFilterQueryBase<UserFilter>,
	IRequest<GetAllUsersQueryResponse>
{
}
