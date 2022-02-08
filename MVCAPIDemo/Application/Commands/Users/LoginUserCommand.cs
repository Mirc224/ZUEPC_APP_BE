﻿using MediatR;
using MVCAPIDemo.Application.Domain;

namespace MVCAPIDemo.Application.Commands.Users;

public class LoginUserCommand: IRequest<User>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
