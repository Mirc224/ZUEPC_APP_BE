using MediatR;

namespace MVCAPIDemo.Auth.Commands;

public class LogoutUserCommand: IRequest<LogoutUserCommandResponse>
{
    public string? JwtId { get; set; }
    public int UserId { get; set; }
}
