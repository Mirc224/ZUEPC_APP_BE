using MVCAPIDemo.Application.Domain;
using MVCAPIDemo.Application.Responses;

namespace MVCAPIDemo.Application.Queries.Users;

public class GetUserQueryResponse : ResponseBase
{
	public User User { get; set; }
}
