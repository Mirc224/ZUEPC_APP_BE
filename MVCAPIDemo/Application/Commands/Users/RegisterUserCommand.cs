using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Commands.Users;

public class RegisterUserCommand: IRequest<User>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
