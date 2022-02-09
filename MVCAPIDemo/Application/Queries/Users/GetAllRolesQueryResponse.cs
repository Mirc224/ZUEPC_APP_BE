using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetAllRolesQueryResponse
{
	public IEnumerable<Role> Roles { get; set; }
	public bool Success { get; set; }

}
