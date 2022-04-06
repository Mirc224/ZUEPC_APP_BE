using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Auth.Commands.AuthActions;

public class LoginUserCommandResponse : ResponseBase
{
    public string? Token { get; set; }
	public string? RefreshToken { get; set; }
}
