using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Commands.Users;

public class RegisterUserCommandResponse
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; } = null;
}
