using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Auth.Commands.AuthActions;

public class LogoutUserCommandResponse : ResponseBase
{
	public string? RefreshToken { get; set; }
}
