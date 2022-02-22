using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Auth.Commands;

public class LoginUserCommandResponse : ResponseBase
{
    public string? Token { get; set; }
	public string? RefreshToken { get; set; }
}
