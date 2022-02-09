using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetAllUsersQueryResponse
{
	public IEnumerable<User> Users { get; set; }
	public bool Success { get; set; }

}
