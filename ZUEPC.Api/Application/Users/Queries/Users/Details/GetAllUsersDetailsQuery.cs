using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Api.Application.Users.Queries.Users.Details;

public class GetAllUsersDetailsQuery:
	PaginationQueryWithUriBase,
	IRequest<GetAllUsersDetailsQueryResponse>
{
}
