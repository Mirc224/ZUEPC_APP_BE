using AutoMapper;
using DataAccess.Data;
using DataAccess.Models;
using MediatR;
using MVCAPIDemo.Application.Domain;
using System.Security.Cryptography;

namespace MVCAPIDemo.Application.Commands.Users;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, User>
{
    private readonly IUserData _repository;
    private readonly IMapper _mapper;

    public LoginUserCommandHandler(IMapper mapper, IUserData repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<User> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {

        

        return null;
    }
}
