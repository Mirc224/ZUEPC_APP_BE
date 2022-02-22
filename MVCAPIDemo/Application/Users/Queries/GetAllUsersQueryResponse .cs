using ZUEPC.Common.Responses;
using Users.Base.Domain;

namespace ZUEPC.Application.Users.Queries;

public class GetAllUsersQueryResponse : ResponseBase
{
	public IEnumerable<User>? Users { get; set; }
}
