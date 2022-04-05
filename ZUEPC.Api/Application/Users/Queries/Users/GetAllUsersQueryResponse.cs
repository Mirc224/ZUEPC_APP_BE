using Users.Base.Domain;
using ZUEPC.Base.Responses;

namespace ZUEPC.Api.Application.Users.Queries.Users;

public class GetAllUsersQueryResponse : PaginatedResponseBase<IEnumerable<User>>
{
}