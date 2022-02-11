using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Commands.Auth;

public class LogoutUserCommand: IRequest<LogoutUserCommandResponse>
{
    public string JwtId { get; set; }
}
