using AutoMapper;
using DataAccess.Data.User;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Security.Cryptography;
using Users.Base.Domain;
using ZUEPC.Api.Application.Users.Queries.Users;
using ZUEPC.Application.Users.Queries.Users;
using ZUEPC.Auth.Domain;
using ZUEPC.Auth.Services;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Localization;

namespace ZUEPC.Application.Auth.Commands.Users;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;
	private readonly IStringLocalizer<DataAnnotations> _localizer;
	private readonly AuthenticationService _jwtAuthenticationService;


	public LoginUserCommandHandler(
		IMapper mapper,
		IMediator mediator,
		IStringLocalizer<DataAnnotations> localizer,
		AuthenticationService jwtAuthenticationService)
	{
		_mapper = mapper;
		_mediator = mediator;
		_localizer = localizer;
		_jwtAuthenticationService = jwtAuthenticationService;
	}

	public async Task<LoginUserCommandResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
	{
		if (request.Email is null || request.Password is null)
		{
			return new()
			{
				ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.INVALID_EMAIL_OR_PASSWORD].Value },
				Success = false
			};
		}

		UserModel? userModel = (await _mediator.Send(new GetUserModelByEmailQuery() { UserEmail = request.Email })).Data;

		if (userModel is null ||
			!VerifyPasswordHash(request.Password, userModel.PasswordHash, userModel.PasswordSalt))
		{
			return new()
			{
				ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.INVALID_EMAIL_OR_PASSWORD].Value },
				Success = false
			};
		};

		User mappedUser = _mapper.Map<User>(userModel);

		AuthResult? authResult = await _jwtAuthenticationService.GenerateJwtToken(mappedUser);
		LoginUserCommandResponse? response = _mapper.Map<LoginUserCommandResponse>(authResult);
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
