using ZUEPC.Common.Responses;
using Users.Base.Application.Domain;

namespace ZUEPC.Application.Users.Queries;

public class GetAllRolesQueryResponse: ResponseBase
{
	public IEnumerable<Role>? Roles { get; set; }
}
