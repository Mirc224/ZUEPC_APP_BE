using Constants.Infrastructure;
using DataAccess.Data.User;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZUEPC.Auth.Domain;
using ZUEPC.DataAccess.Models.Users;
using ZUEPC.Localization;
using ZUEPC.Options;

namespace ZUEPC.Auth.Services;

public class AuthenticationService
{
	private readonly JwtSettings _jwtSettings;
	private readonly TokenValidationParameters _tokenValidationParams;
	private readonly IUserData _userRepository;
	private readonly IStringLocalizer<DataAnnotations> _localizer;
	private readonly string _usedSecuritySignatureAlgorithm = SecurityAlgorithms.HmacSha512Signature;
	private readonly string _usedSecurityAlgorithm = SecurityAlgorithms.HmacSha512;

	public AuthenticationService(
		IOptions<JwtSettings> jwtSettings,
		TokenValidationParameters tokenValidationParams,
		IUserData userRepository,
		IStringLocalizer<DataAnnotations> localizer)
	{
		_jwtSettings = jwtSettings.Value;
		_userRepository = userRepository;
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

			if (!IsExpired(tokenInVerification))
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_EXPIRED] } };
			}

			if (refreshToken is null)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_EXIST] } };
			}

			RefreshTokenModel? storedToken = await _userRepository.GetRefreshTokenByTokenAsync(refreshToken);

			if (storedToken is null)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_EXIST] } };
			}

			if (storedToken.ExpiryDate.ToUniversalTime() < DateTime.UtcNow)
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
			int rowsUpadted = await _userRepository.UpdateRefreshTokenAsync(storedToken);

			if (rowsUpadted != 1)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.UNKNOWN_DB_ERROR] } };
			}

			UserModel? userModel = await _userRepository.GetUserByIdAsync(storedToken.UserId);

			if (userModel is null)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.USER_NOT_EXIST] } };
			}

			return await GenerateJwtToken(userModel);
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

		RefreshTokenModel? storedToken = await _userRepository.GetRefreshTokenByTokenAsync(refreshToken);

		if (storedToken is null)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_NOT_EXIST] } };
		}

		if (storedToken.IsRevoked)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.TOKEN_IS_REVOKED] } };
		}

		storedToken.IsRevoked = true;
		int rowsUpadted = await _userRepository.UpdateRefreshTokenAsync(storedToken);

		if (rowsUpadted != 1)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.UNKNOWN_DB_ERROR] } };
		}

		return new() { Success = true };
	}

	public async Task<RevokeResult> RevokeUserTokenAsync(int userId, string jwtId)
	{
		RefreshTokenModel? storedToken = await _userRepository.GetUserRefreshByJwtIdAsync(jwtId);

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
		int rowsUpadted = await _userRepository.UpdateRefreshTokenAsync(storedToken);

		if (rowsUpadted != 1)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer[DataAnnotationsKeyConstants.UNKNOWN_DB_ERROR] } };
		}

		return new() { Success = true };
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

	public async Task<AuthResult> GenerateJwtToken(UserModel userModel)
	{
		JwtSecurityTokenHandler jwtTokenHandler = new();

		SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

		SecurityTokenDescriptor tokenDescriptor = new()
		{
			Subject = new ClaimsIdentity(await GetUserClaims(userModel)),
			Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
			SigningCredentials = new SigningCredentials(key, _usedSecuritySignatureAlgorithm)
		};

		SecurityToken token = jwtTokenHandler.CreateToken(tokenDescriptor);
		string jwtToken = jwtTokenHandler.WriteToken(token);

		RefreshTokenModel refreshToken = new()
		{
			JwtId = token.Id,
			IsUsed = false,
			IsRevoked = false,
			UserId = userModel.Id,
			CreatedAt = DateTime.UtcNow,
			ExpiryDate = DateTime.UtcNow.AddMonths(6),
			Token = Guid.NewGuid()
		};

		await _userRepository.InsertRefreshTokenAsync(refreshToken);

		return new() { Token = jwtToken, Success = true, RefreshToken = refreshToken.Token };
	}

	private async Task<List<Claim>> GetUserClaims(UserModel userModel)
	{
		List<Claim> claims = new()
		{
				new Claim(CustomClaims.UserId, userModel.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, userModel.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		IEnumerable<RoleModel> userRoles = await _userRepository.GetUserRolesAsync(userModel.Id);
		claims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x.Id.ToString())).ToList());
		return claims;
	}
}
