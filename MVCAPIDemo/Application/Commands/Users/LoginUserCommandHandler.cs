using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Services;
using MVCAPIDemo.Localization;
using System.Security.Cryptography;

namespace MVCAPIDemo.Application.Commands.Users;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
{
    private readonly IUserData _repository;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<DataAnnotations> _localizer;
    private readonly JwtAuthenticationService _jwtAuthenticationService;


    public LoginUserCommandHandler(
        IMapper mapper,
        IUserData repository,
        IStringLocalizer<DataAnnotations> localizer,
        JwtAuthenticationService jwtAuthenticationService)
    {
        _repository = repository;
        _mapper = mapper;
        _localizer = localizer;
        _jwtAuthenticationService = jwtAuthenticationService;
    }

    public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var userModel = await _repository.GetUserByEmail(request.Email);

        if (userModel is null ||
            !VerifyPasswordHash(request.Password, userModel.PasswordHash, userModel.PasswordSalt))
        {
            return new() 
			{ 
				ErrorMessages = new string[] { _localizer["InvalidEmailOrPassword"].Value },
				Success = false
			};
        };

        string token = _jwtAuthenticationService.CreateToken(userModel);
        return new() { Token = token, Success = true };
    }

    private byte[] GetHash(string password, byte[] salt)
    {
        using HMACSHA512 hmac = new(salt);
        return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); ;
    }

    private bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
    {
        byte[] passwordHash = GetHash(password, userPasswordSalt);
        return Convert.ToBase64String(passwordHash) == Convert.ToBase64String(userPasswordHash);
    }
}
