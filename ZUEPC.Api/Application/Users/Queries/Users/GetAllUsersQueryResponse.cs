using Users.Base.Domain;
using ZUEPC.Responses;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetAllUsersQueryResponse : PagedResponseBase<IEnumerable<User>>
{
}