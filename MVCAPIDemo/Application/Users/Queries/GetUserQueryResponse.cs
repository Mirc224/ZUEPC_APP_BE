using ZUEPC.Common.Responses;
using Users.Base.Domain;

namespace ZUEPC.Application.Users.Queries;

public class GetUserQueryResponse : ResponseBase
{
	public User? User { get; set; }
}
