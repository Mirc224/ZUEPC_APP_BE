using AutoMapper;
using Dapper;
using DataAccess.Data.User;
using MediatR;
using Microsoft.Extensions.Localization;
using MVCAPIDemo.Application.Services;
using MVCAPIDemo.Localization;
using System.Security.Cryptography;

namespace MVCAPIDemo.Application.Commands.Auth;

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
		var builder = new SqlBuilder();
		builder.Select("*");
		builder.Where("Email = @Email");
        var userModel = (await _repository.GetUsersAsync(new { request.Email }, builder)).FirstOrDefault();

        if (userModel is null ||
            !VerifyPasswordHash(request.Password, userModel.PasswordHash, userModel.PasswordSalt))
        {
            return new() 
			{ 
				ErrorMessages = new string[] { _localizer["InvalidEmailOrPassword"].Value },
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

    private bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
    {
        byte[] passwordHash = GetHash(password, userPasswordSalt);
        return Convert.ToBase64String(passwordHash) == Convert.ToBase64String(userPasswordHash);
    }
}
