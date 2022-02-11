using MVCAPIDemo.Application.Responses;

namespace MVCAPIDemo.Application.Commands.Auth;

public class LogoutUserCommandResponse : ResponseBase
{
	public string RefreshToken { get; set; }
}
