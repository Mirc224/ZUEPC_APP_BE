using MVCAPIDemo.Application.Responses;

namespace MVCAPIDemo.Application.Commands.Auth;

public class RefreshTokenCommandResponse : CQRSBaseResponse
{
	public string Token { get; set; }
	public string RefreshToken { get; set; }
}
