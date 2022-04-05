using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Auth.Commands.Users;

public class LogoutUserCommandResponse : ResponseBase
{
	public string? RefreshToken { get; set; }
}
