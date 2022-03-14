using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Api.Application.Users.Queries.Users.Details;

public class GetAllUsersDetailsQuery:
	PaginationWithFilterQueryBase<UserFilter>,
	IRequest<GetAllUsersDetailsQueryResponse>
{
}
