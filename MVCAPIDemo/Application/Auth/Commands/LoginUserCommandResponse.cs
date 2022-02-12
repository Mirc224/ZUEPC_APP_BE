using MVCAPIDemo.Common.Responses;

namespace MVCAPIDemo.Auth.Commands;

public class LoginUserCommandResponse : ResponseBase
{
    public string? Token { get; set; }
	public string? RefreshToken { get; set; }
}
