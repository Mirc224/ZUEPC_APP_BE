using MVCAPIDemo.Common.Responses;
using Users.Base.Domain;

namespace MVCAPIDemo.Auth.Commands;

public class RegisterUserCommandResponse : ResponseBase
{
	public User? CreatedUser { get; set; }
}
