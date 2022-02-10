using AutoMapper;
using DataAccess.Data.User;
using DataAccess.Enums;
using DataAccess.Models;
using MediatR;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Localization;
using System.Security.Cryptography;

namespace MVCAPIDemo.Application.Commands.Users;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
{
    private readonly IUserData _repository;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<DataAnnotations> _localizer;

    public RegisterUserCommandHandler(IMapper mapper, IUserData repository, IStringLocalizer<DataAnnotations> localizer)
    {
        _repository = repository;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        byte[]? passwordHash = null;
        byte[]? passwordSalt = null;

        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
        }

        var user = _mapper.Map<UserModel>(request);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        int newUserId = await _repository.InsertUser(user);
        
		if (newUserId == 0)
        {
            return new() 
			{ 
				ErrorMessages = new string[] { _localizer["UnknownError"] },
				Success = false 
			};
        }
		await _repository.InsertUserRole(newUserId, RolesType.USER);

        return new RegisterUserCommandResponse() { Success = true};
    }
}
