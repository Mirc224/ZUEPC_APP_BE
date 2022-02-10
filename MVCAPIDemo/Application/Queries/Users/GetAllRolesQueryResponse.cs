using MVCAPIDemo.Application.Domain;
using MVCAPIDemo.Application.Responses;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetAllRolesQueryResponse: CQRSBaseResponse
{
	public IEnumerable<Role> Roles { get; set; }
}
