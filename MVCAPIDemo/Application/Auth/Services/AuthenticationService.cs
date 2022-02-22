using Constants.Infrastructure;
using DataAccess.Data.User;
using DataAccess.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZUEPC.Auth.Domain;
using ZUEPC.Localization;
using ZUEPC.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
		var jwtTokenHandler = new JwtSecurityTokenHandler();
		try
		{
			var tokenInVerification = jwtTokenHandler.ValidateToken(token, _tokenValidationParams, out var validatedToken);
			if (!IsJwtSecurityToken(validatedToken))
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer["NotValidToken"] } };
			}

			if (!IsExpired(tokenInVerification))
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer["NotExpiredToken"] } };
			}

			if (refreshToken is null)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer["TokenNotExist"] } };
			}

			var storedToken = await _userRepository.GetRefreshTokenByTokenAsync(refreshToken);

			if (storedToken is null)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer["TokenNotExist"] } };
			}

			if (storedToken.ExpiryDate.ToUniversalTime() < DateTime.UtcNow)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer["RefreshTokenExpired"] } };
			}

			if (storedToken.IsUsed)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer["TokenIsUsed"] } };
			}

			if (storedToken.IsRevoked)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer["TokenIsRevoked"] } };
			}

			var jtiClaim = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);

			if (jtiClaim is null)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer["NotValidToken"] } };
			}

			var jti = jtiClaim.Value;

			if (storedToken.JwtId != jti)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer["TokenNotMatch"] } };
			}

			storedToken.IsUsed = true;
			int rowsUpadted = await _userRepository.UpdateRefreshTokenAsync(storedToken);

			if (rowsUpadted != 1)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer["UnknownDbError"] } };
			}

			var userModel = await _userRepository.GetUserByIdAsync(storedToken.UserId);

			if (userModel is null)
			{
				return new() { Success = false, ErrorMessages = new string[] { _localizer["UserNotFound"] } };
			}

			return await GenerateJwtToken(userModel);
		}
		catch (Exception)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer["NotValidToken"] } };
		}
	}

	public async Task<RevokeResult> RevokeTokenAsync(string? refreshToken)
	{
		if (refreshToken is null)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer["TokenNotExist"] } };
		}

		var storedToken = await _userRepository.GetRefreshTokenByTokenAsync(refreshToken);

		if (storedToken is null)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer["TokenNotExist"] } };
		}

		if (storedToken.IsRevoked)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer["TokenIsRevoked"] } };
		}

		storedToken.IsRevoked = true;
		int rowsUpadted = await _userRepository.UpdateRefreshTokenAsync(storedToken);

		if (rowsUpadted != 1)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer["UnknownDbError"] } };
		}

		return new() { Success = true };
	}

	public async Task<RevokeResult> RevokeUserTokenAsync(int userId, string jwtId)
	{
		var storedToken = await _userRepository.GetUserRefreshByJwtIdAsync(jwtId);

		if (storedToken is null)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer["TokenNotExist"] } };
		}

		if (storedToken.UserId != userId)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer["TokenNotOwnedByUser"] } };
		}

		if (storedToken.IsRevoked)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer["TokenIsRevoked"] } };
		}

		storedToken.IsRevoked = true;
		int rowsUpadted = await _userRepository.UpdateRefreshTokenAsync(storedToken);

		if (rowsUpadted != 1)
		{
			return new() { Success = false, ErrorMessages = new string[] { _localizer["UnknownDbError"] } };
		}

		return new() { Success = true };
	}

	private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
	{
		var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp);
		return dateTimeVal;
	}

	private bool IsExpired(ClaimsPrincipal tokenInVerification)
	{
		var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
		var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

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
			var result = jwtSecurityToken.Header.Alg.Equals(_usedSecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase);
			return result;
		}

		return false;
	}

	public async Task<AuthResult> GenerateJwtToken(UserModel userModel)
	{
		var jwtTokenHandler = new JwtSecurityTokenHandler();

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(await GetUserClaims(userModel)),
			Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
			SigningCredentials = new SigningCredentials(key, _usedSecuritySignatureAlgorithm)
		};

		var token = jwtTokenHandler.CreateToken(tokenDescriptor);
		var jwtToken = jwtTokenHandler.WriteToken(token);

		var refreshToken = new RefreshTokenModel()
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
		var claims = new List<Claim>()
		{
				new Claim(CustomClaims.UserId, userModel.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, userModel.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		var userRoles = await _userRepository.GetUserRolesAsync(userModel.Id);
		claims.AddRange(userRoles.Select(x=> new Claim(ClaimTypes.Role, x.Id.ToString())).ToList());
		return claims;
	}

	private string RandomString(int length)
	{
		var random = new Random();
		string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[random.Next(s.Length)]).ToArray());
	}
}
