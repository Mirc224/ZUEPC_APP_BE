using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Commands.Users;

public class LoginUserCommandResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
}
