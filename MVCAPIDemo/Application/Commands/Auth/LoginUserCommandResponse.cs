using MVCAPIDemo.Application.Responses;

namespace MVCAPIDemo.Application.Commands.Auth;

public class LoginUserCommandResponse : ResponseBase
{
    public string Token { get; set; }
	public string RefreshToken { get; set; }
}
