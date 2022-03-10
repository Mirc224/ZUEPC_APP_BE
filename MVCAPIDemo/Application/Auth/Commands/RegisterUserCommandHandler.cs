using AutoMapper;
using DataAccess.Data.User;
using ZUEPC.DataAccess.Enums;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Security.Cryptography;
using Users.Base.Domain;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Localization;

namespace ZUEPC.Application.Auth.Commands;

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
		if(request.Password is null)
		{
			return new RegisterUserCommandResponse() { Success = false };
		}

        byte[]? passwordHash = null;
        byte[]? passwordSalt = null;

        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
        }

        var newUserModel = _mapper.Map<UserModel>(request);
        newUserModel.PasswordHash = passwordHash;
        newUserModel.PasswordSalt = passwordSalt;

        int newUserId = await _repository.InsertUserAsync(newUserModel);

		if (newUserId == 0)
        {
            return new() 
			{ 
				ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.UNKNOWN_ERROR] },
				Success = false 
			};
        }
		newUserModel.Id = newUserId;

		var defaultUserRoles = new List<RoleType>() { RoleType.USER };

		var newUser = _mapper.Map<User>(newUserModel);

		newUser.Roles = defaultUserRoles;

		foreach(var role in defaultUserRoles)
		{
			await _repository.InsertUserRoleAsync(newUserId, role);
		}


        return new RegisterUserCommandResponse() { Success = true, CreatedUser = newUser};
    }
}
