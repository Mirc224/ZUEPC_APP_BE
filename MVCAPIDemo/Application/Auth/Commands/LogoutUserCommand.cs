using MediatR;

namespace ZUEPC.Application.Auth.Commands;

public class LogoutUserCommand: IRequest<LogoutUserCommandResponse>
{
    public string? JwtId { get; set; }
    public int UserId { get; set; }
}
