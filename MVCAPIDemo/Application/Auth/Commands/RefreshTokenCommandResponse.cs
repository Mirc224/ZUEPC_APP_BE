using MVCAPIDemo.Common.Responses;

namespace MVCAPIDemo.Auth.Commands;

public class RefreshTokenCommandResponse : ResponseBase
{
	public string? Token { get; set; }
	public string? RefreshToken { get; set; }
}
