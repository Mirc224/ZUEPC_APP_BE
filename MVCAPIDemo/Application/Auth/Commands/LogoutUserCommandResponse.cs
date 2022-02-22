using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Auth.Commands;

public class LogoutUserCommandResponse : ResponseBase
{
	public string? RefreshToken { get; set; }
}
