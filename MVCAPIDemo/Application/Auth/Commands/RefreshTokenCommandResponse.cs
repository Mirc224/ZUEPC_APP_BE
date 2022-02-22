using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Auth.Commands;

public class RefreshTokenCommandResponse : ResponseBase
{
	public string? Token { get; set; }
	public string? RefreshToken { get; set; }
}
