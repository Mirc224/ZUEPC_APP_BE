using ZUEPC.Api.Application.Users.Entities.Details;
using ZUEPC.Responses;

namespace ZUEPC.Api.Application.Users.Queries.Users.Details;

public class GetAllUsersDetailsQueryResponse : PagedResponseBase<IEnumerable<UserDetails>>
{
}