using MediatR;

namespace ZUEPC.Application.Auth.Commands;

public class RegisterUserCommand: IRequest<RegisterUserCommandResponse>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
