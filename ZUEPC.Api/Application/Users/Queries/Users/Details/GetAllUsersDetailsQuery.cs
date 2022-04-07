using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Api.Application.Users.Queries.Users.Details;

public class GetAllUsersDetailsQuery:
	PaginatedQueryWithFilterBase<UserFilter>,
	IRequest<GetAllUsersDetailsQueryResponse>
{
}
