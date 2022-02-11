using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data.User;

public partial class UserData : IUserData
{
	public Task<Guid> InsertRefreshTokenAsync(RefreshTokenModel refreshToken)
	{
		string sql = $@"INSERT INTO {_refreshTokensTableName} (UserId, Token, JwtId, IsUsed, IsRevoked, CreatedAt, ExpiryDate)
						OUTPUT INSERTED.Token
						VALUES (@UserId, @Token, @JwtId, @IsUsed, @IsRevoked, @CreatedAt, @ExpiryDate)";

		return _db.ExecuteScalarAsync<Guid, RefreshTokenModel>(sql, refreshToken);
	}

	public async Task<RefreshTokenModel?> GetRefreshTokenByTokenAsync(string refreshToken)
	{
		string sql = $@"SELECT * FROM {_refreshTokensTableName} 
						WHERE Token = @Token";
		return (await _db.QueryAsync<RefreshTokenModel, dynamic>(sql, new { Token = refreshToken })).FirstOrDefault();
	}

	public async Task<RefreshTokenModel?> GetRefreshTokenByJwtIdAsync(string jwtId)
	{
		string sql = $@"SELECT * FROM {_refreshTokensTableName} 
						WHERE JwtId = @JwtId";
		return (await _db.QueryAsync<RefreshTokenModel, dynamic>(sql, new { JwtId = jwtId })).FirstOrDefault();
	}

	public async Task<int> UpdateRefreshTokenAsync(RefreshTokenModel refreshTokenModel)
	{
		string sql = $@"UPDATE {_refreshTokensTableName} 
						SET UserId = @UserId, JwtId = @JwtId, IsUsed = @IsUsed, 
						IsRevoked = @IsRevoked, CreatedAt = @CreatedAt, ExpiryDate = @ExpiryDate
						WHERE Token = @Token";	
		return await _db.ExecuteAsync(sql, refreshTokenModel);
	}

	public async Task<int> UpdateRefreshTokenByJwtIdAsync(RefreshTokenModel refreshTokenModel)
	{
		string sql = $@"UPDATE {_refreshTokensTableName} 
						SET UserId = @UserId, JwtId = @JwtId, IsUsed = @IsUsed, 
						IsRevoked = @IsRevoked, CreatedAt = @CreatedAt, ExpiryDate = @ExpiryDate
						WHERE JwtId = @JwtId";
		return await _db.ExecuteAsync(sql, refreshTokenModel);
	}
}
