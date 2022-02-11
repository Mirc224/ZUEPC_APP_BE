using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data.User;

public partial class UserData : IUserData
{
	public Task<int> InsertRefreshTokenAsync(RefreshTokenModel refreshToken)
	{
		string sql = $@"INSERT INTO {_refreshTokensTableName} (UserId, Token, JwtId, IsUsed, IsRevoked, CreatedAt, ExpiryDate)
						OUTPUT INSERTED.Id
						VALUES (@UserId, @Token, @JwtId, @IsUsed, @IsRevoked, @CreatedAt, @ExpiryDate)";

		return _db.ExecuteScalarAsync<int, RefreshTokenModel>(sql, refreshToken);
	}

	public async Task<RefreshTokenModel?> GetRefreshTokenByTokenAsync(string refreshToken)
	{
		string sql = $@"SELECT * FROM {_refreshTokensTableName} 
						WHERE Token = @Token";
		var refreshTokenModel = (await _db.QueryAsync<RefreshTokenModel, dynamic>(sql, new { Token = refreshToken })).FirstOrDefault();
		return refreshTokenModel;
	}

	public async Task<int> UpdateRefreshTokenAsync(RefreshTokenModel refreshTokenModel)
	{
		string sql = $@"UPDATE {_refreshTokensTableName} 
						SET UserId = @UserId, Token = @Token, JwtId = @JwtId, IsUsed = @IsUsed, 
						IsRevoked = @IsRevoked, CreatedAt = @CreatedAt, ExpiryDate = @ExpiryDate
						WHERE Id = @Id";
		
		return await _db.ExecuteAsync(sql, refreshTokenModel);
	}
}
