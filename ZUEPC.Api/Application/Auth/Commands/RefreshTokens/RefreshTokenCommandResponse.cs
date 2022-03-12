using ZUEPC.Responses;

namespace ZUEPC.Application.Auth.Commands.RefreshTokens;

public class RefreshTokenCommandResponse : ResponseBase
{
	public string? Token { get; set; }
	public string? RefreshToken { get; set; }
}
