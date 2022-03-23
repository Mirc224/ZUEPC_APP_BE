using AutoMapper;
using Constants.Infrastructure;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Base.Domain;
using ZUEPC.Api.Application.Auth.Commands.RefreshTokens;
using ZUEPC.Api.Application.Auth.Queries.RefreshTokens;
using ZUEPC.Api.Application.Users.Queries.UserRoles;
using ZUEPC.Application.Users.Queries.Users;
using ZUEPC.Auth.Domain;
using ZUEPC.Base.Enums.Users;
using ZUEPC.Common.Extensions;
using ZUEPC.Localization;
using ZUEPC.Options;
using ZUEPC.Users.Base.Domain;

namespace ZUEPC.Auth.Services;

public class AuthenticationService
{
	private readonly JwtSettings _jwtSettings;
	private readonly TokenValidationParameters _tokenValidationParams;
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;
	private readonly IStringLocalizer<DataAnnotations> _localizer;
	private readonly string _usedSecuritySignatureAlgorithm = SecurityAlgorithms.HmacSha512Signature;
	private readonly string _usedSecurityAlgorithm = SecurityAlgorithms.HmacSha512;

	public AuthenticationService(
		IMapper mapper,
		IMediator mediator,
		IOptions<JwtSettings> jwtSettings,
		TokenValidationParameters tokenValidationParams,
		IStringLocalizer<DataAnnotations> localizer)
	{
		_jwtSettings = jwtSettings.Value;
		_mapper = mapper;
		_mediator = mediator;
		_localizer = localizer;
		_tokenValidationParams = tokenValidationParams.Clone();
		_tokenValidationParams.ValidateLifetime = false;
	}

	public async Task<AuthResult> VerifyAndGenerateTokenAsync(string? token, string? refreshToken)
	{
		JwtSecurityTokenHandler jwtTokenHandler = new();
		try
		{
			ClaimsPrincipal? tokenInVerification = jwtTokenHandler.ValidateToken(token, _tokenValidationParams, out SecurityToken? validatedToken);
			if (!IsJwtSecurityToken(validatedToken))
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_VALID] } };
			}

			//if (!IsExpired(tokenInVerification))
			//{
			//	return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_EXPIRED] } };
			//}

			if (refreshToken is null)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_EXIST] } };
			}

			RefreshToken? storedToken = (await _mediator.Send(new GetRefreshTokenByRefreshTokenQuery() { RefreshToken = refreshToken })).Data;

			if (storedToken is null)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_EXIST] } };
			}

			if (storedToken.ExpiryDate < DateTime.UtcNow)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.REFRESH_TOKEN_EXPRIRED] } };
			}

			if (storedToken.IsUsed)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_IS_USED] } };
			}

			if (storedToken.IsRevoked)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_IS_REVOKED] } };
			}

			Claim? jtiClaim = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);

			if (jtiClaim is null)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_VALID] } };
			}

			string? jti = jtiClaim.Value;

			if (storedToken.JwtId != jti)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_MATCH] } };
			}

			storedToken.IsUsed = true;

			UpdateRefreshTokenCommandResponse updateResponse = await UpdateRefreshTokenCommand(storedToken);

			if (!updateResponse.Success)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.UNKNOWN_DB_ERROR] } };
			}

			User? user = (await _mediator.Send(new GetUserQuery() { Id = storedToken.UserId })).Data;

			if (user is null)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.USER_NOT_EXIST] } };
			}

			return await GenerateJwtToken(user);
		}
		catch (Exception)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_VALID] } };
		}
	}

	public async Task<RevokeResult> RevokeTokenAsync(string? refreshToken)
	{
		if (refreshToken is null)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_EXIST] } };
		}

		RefreshToken? storedToken = (await _mediator.Send(new GetRefreshTokenByRefreshTokenQuery() { RefreshToken = refreshToken })).Data;

		if (storedToken is null)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_EXIST] } };
		}

		if (storedToken.IsRevoked)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_IS_REVOKED] } };
		}

		storedToken.IsRevoked = true;

		UpdateRefreshTokenCommandResponse updateResponse = await UpdateRefreshTokenCommand(storedToken);

		if (!updateResponse.Success)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.UNKNOWN_DB_ERROR] } };
		}

		return new() { Success = true };
	}

	public async Task<RevokeResult> RevokeUserTokenAsync(int userId, string jwtId)
	{
		RefreshToken? storedToken = (await _mediator.Send(new GetRefreshTokenByJwtIdQuery() { JwtId = jwtId })).Data;

		if (storedToken is null)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_EXIST] } };
		}

		if (storedToken.UserId != userId)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_OWNED_BY_USER] } };
		}

		if (storedToken.IsRevoked)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_IS_REVOKED] } };
		}

		storedToken.IsRevoked = true;
		UpdateRefreshTokenCommandResponse updateResponse = await UpdateRefreshTokenCommand(storedToken);

		if (!updateResponse.Success)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.UNKNOWN_DB_ERROR] } };
		}

		return new() { Success = true };
	}

	private async Task<UpdateRefreshTokenCommandResponse> UpdateRefreshTokenCommand(RefreshToken token)
	{
		UpdateRefreshTokenCommand updateTokenCommand = _mapper.Map<UpdateRefreshTokenCommand>(token);
		return await _mediator.Send(updateTokenCommand);
	}

	private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
	{
		DateTime dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp);
		return dateTimeVal;
	}

	private bool IsExpired(ClaimsPrincipal tokenInVerification)
	{
		long utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
		DateTime expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

		if (expiryDate > DateTime.UtcNow)
		{
			return false;
		}
		return true;
	}

	private bool IsJwtSecurityToken(SecurityToken validatedToken)
	{
		if (validatedToken is JwtSecurityToken jwtSecurityToken)
		{
			bool result = jwtSecurityToken.Header.Alg.Equals(_usedSecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase);
			return result;
		}

		return false;
	}

	public async Task<AuthResult> GenerateJwtToken(User user)
	{
		JwtSecurityTokenHandler jwtTokenHandler = new();

		SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

		SecurityTokenDescriptor tokenDescriptor = new()
		{
			Subject = new ClaimsIdentity(await GetUserClaims(user)),
			Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
			SigningCredentials = new SigningCredentials(key, _usedSecuritySignatureAlgorithm)
		};

		SecurityToken token = jwtTokenHandler.CreateToken(tokenDescriptor);
		string jwtToken = jwtTokenHandler.WriteToken(token);

		RefreshToken refreshToken = new()
		{
			JwtId = token.Id,
			IsUsed = false,
			IsRevoked = false,
			UserId = user.Id,
			CreatedAt = DateTime.UtcNow,
			ExpiryDate = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenLifetime),
			Token = Guid.NewGuid().ToString()
		};


		await CreateRefreshToken(refreshToken);

		return new() { Token = jwtToken, Success = true, RefreshToken = refreshToken.Token };
	}

	private async Task<CreateRefreshTokenCommandResponse> CreateRefreshToken(RefreshToken token)
	{
		CreateRefreshTokenCommand createCommand = _mapper.Map<CreateRefreshTokenCommand>(token);
		return await _mediator.Send(createCommand);
	}

	private async Task<List<Claim>> GetUserClaims(User user)
	{
		List<Claim> claims = new()
		{
			new Claim(CustomClaims.UserId, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.Email, user.Email),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		GetUserRolesByUserIdQueryResponse userRolesResponse = (await _mediator.Send(new GetUserRolesByUserIdQuery() { UserId = user.Id}));
		if(userRolesResponse.Success)
		{
			IEnumerable<UserRole> userRoles = userRolesResponse.Data;
			foreach(UserRole userRole in userRoles.OrEmptyIfNull())
			{
				claims.Add(new Claim(ClaimTypes.Role, (userRole.RoleId).ToString()));
			}
		}
		return claims;
	}
}
