using AutoMapper;
using Dapper;
using DataAccess.Data.User;
using MediatR;
using Microsoft.Extensions.Localization;
using ZUEPC.Auth.Services;
using ZUEPC.Localization;
using System.Security.Cryptography;

namespace ZUEPC.Application.Auth.Commands;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
{
    private readonly IUserData _repository;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<DataAnnotations> _localizer;
    private readonly AuthenticationService _jwtAuthenticationService;


    public LoginUserCommandHandler(
        IMapper mapper,
        IUserData repository,
        IStringLocalizer<DataAnnotations> localizer,
        AuthenticationService jwtAuthenticationService)
    {
        _repository = repository;
        _mapper = mapper;
        _localizer = localizer;
        _jwtAuthenticationService = jwtAuthenticationService;
    }

    public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
		if(request.Email is null || request.Password is null)
		{
			return new()
			{
				ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.INVALID_EMAIL_OR_PASSWORD].Value },
				Success = false
			};
		}

        var userModel = await _repository.GetUserByEmailAsync(request.Email);

        if (userModel is null ||
            !VerifyPasswordHash(request.Password, userModel.PasswordHash, userModel.PasswordSalt))
        {
            return new() 
			{ 
				ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.INVALID_EMAIL_OR_PASSWORD].Value },
				Success = false
			};
        };

        var authResult = await _jwtAuthenticationService.GenerateJwtToken(userModel);
		var response = _mapper.Map<LoginUserCommandResponse>(authResult);
        return response;
    }

    private byte[] GetHash(string password, byte[] salt)
    {
        using HMACSHA512 hmac = new(salt);
        return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); ;
    }

    private bool VerifyPasswordHash(string? password, byte[] userPasswordHash, byte[] userPasswordSalt)
    {
		if (password is null)
		{
			return false;
		}

        byte[] passwordHash = GetHash(password, userPasswordSalt);
        return Convert.ToBase64String(passwordHash) == Convert.ToBase64String(userPasswordHash);
    }
}
