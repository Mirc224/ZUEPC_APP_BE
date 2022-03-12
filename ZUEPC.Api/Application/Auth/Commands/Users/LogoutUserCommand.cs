using MediatR;

namespace ZUEPC.Application.Auth.Commands.Users;

public class LogoutUserCommand: IRequest<LogoutUserCommandResponse>
{
    public string? JwtId { get; set; }
    public int UserId { get; set; }
}
