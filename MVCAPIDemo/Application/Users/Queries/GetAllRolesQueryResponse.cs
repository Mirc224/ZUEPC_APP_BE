using MVCAPIDemo.Common.Responses;
using Users.Base.Application.Domain;

namespace MVCAPIDemo.Users.Queries;

public class GetAllRolesQueryResponse: ResponseBase
{
	public IEnumerable<Role>? Roles { get; set; }
}
