using MVCAPIDemo.Common.Responses;
using Users.Base.Domain;

namespace MVCAPIDemo.Users.Queries;

public class GetUserQueryResponse : ResponseBase
{
	public User? User { get; set; }
}
