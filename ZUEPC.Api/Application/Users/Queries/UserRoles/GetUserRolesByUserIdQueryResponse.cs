using ZUEPC.Responses;
using ZUEPC.Users.Domain;

namespace ZUEPC.Api.Application.Users.Queries.UserRoles;

public class GetUserRolesByUserIdQueryResponse : ResponseWithDataBase<IEnumerable<UserRole>>
{
}