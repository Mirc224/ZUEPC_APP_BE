using MediatR;

namespace MVCAPIDemo.Auth.Commands;

public class LoginUserCommand: IRequest<LoginUserCommandResponse>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
