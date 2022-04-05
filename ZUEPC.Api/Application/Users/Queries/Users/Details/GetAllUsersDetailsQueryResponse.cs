using ZUEPC.Api.Application.Users.Entities.Details;
using ZUEPC.Base.Responses;

namespace ZUEPC.Api.Application.Users.Queries.Users.Details;

public class GetAllUsersDetailsQueryResponse : PaginatedResponseBase<IEnumerable<UserDetails>>
{
}