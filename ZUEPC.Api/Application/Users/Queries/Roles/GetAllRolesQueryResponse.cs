using ZUEPC.Base.Responses;
using Users.Base.Application.Domain;

namespace ZUEPC.Application.Users.Queries.Roles;

public class GetAllRolesQueryResponse: ResponseBase
{
	public IEnumerable<Role>? Roles { get; set; }
}
