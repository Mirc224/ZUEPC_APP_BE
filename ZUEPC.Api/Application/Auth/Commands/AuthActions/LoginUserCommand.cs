using MediatR;

namespace ZUEPC.Application.Auth.Commands.AuthActions;

public class LoginUserCommand: IRequest<LoginUserCommandResponse>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
