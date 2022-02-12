using MVCAPIDemo.Common.Responses;
using Users.Base.Domain;

namespace MVCAPIDemo.Users.Queries;

public class GetAllUsersQueryResponse : ResponseBase
{
	public IEnumerable<User>? Users { get; set; }
}
