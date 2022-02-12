using MVCAPIDemo.Common.Responses;

namespace MVCAPIDemo.Auth.Commands;

public class LogoutUserCommandResponse : ResponseBase
{
	public string? RefreshToken { get; set; }
}
