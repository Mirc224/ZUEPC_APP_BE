using MediatR;

namespace ZUEPC.Application.Auth.Commands.AuthActions;

public class LogoutUserCommand: IRequest<LogoutUserCommandResponse>
{
    public string? JwtId { get; set; }
    public long UserId { get; set; }
}
