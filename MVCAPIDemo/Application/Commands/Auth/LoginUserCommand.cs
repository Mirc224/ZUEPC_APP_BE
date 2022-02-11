using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Commands.Auth;

public class LoginUserCommand: IRequest<LoginUserCommandResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
