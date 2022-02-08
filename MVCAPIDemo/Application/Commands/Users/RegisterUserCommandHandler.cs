using AutoMapper;
using DataAccess.Data;
using DataAccess.Models;
using MediatR;
using MVCAPIDemo.Application.Domain;
using System.Security.Cryptography;

namespace MVCAPIDemo.Application.Commands.Users;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
{
    private readonly IUserData _repository;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IMapper mapper, IUserData repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {

        byte[] passwordHash = null;
        byte[] passwordSalt = null;

        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
        }

        var user = _mapper.Map<UserModel>(request);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        int res = await _repository.InsertUser(user);

        return null;
    }
}
