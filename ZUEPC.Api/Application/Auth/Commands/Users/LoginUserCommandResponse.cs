using ZUEPC.Responses;

namespace ZUEPC.Application.Auth.Commands.Users;

public class LoginUserCommandResponse : ResponseBase
{
    public string? Token { get; set; }
	public string? RefreshToken { get; set; }
}
