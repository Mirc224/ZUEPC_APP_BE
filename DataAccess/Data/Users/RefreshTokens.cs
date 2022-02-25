using Dapper;
using ZUEPC.DataAccess.Models.Users;

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
		var builder = new SqlBuilder();
		builder.Where("Token = @Token");
		return (await GetRefreshTokensAsync(new { Token = refreshToken }, builder)).FirstOrDefault();
	}

	public async Task<int> UpdateRefreshTokenAsync(RefreshTokenModel refreshToken)
	{
		var builder = new SqlBuilder();
		builder.Where("Token = @Token");
		return await UpdateRefreshTokenAsync(refreshToken, builder);
	}

	public async Task<int> UpdateRefreshTokenByJwtIdAsync(RefreshTokenModel refreshToken)
	{
		var builder = new SqlBuilder();
		builder.Where("JwtId = @JwtId");
		return await UpdateRefreshTokenAsync(refreshToken, builder);
	}

	public async Task<RefreshTokenModel?> GetUserRefreshTokenAsync(int userId, string jwtId)
	{
		var builder = new SqlBuilder();
		builder.Where("JwtId = @JwtId");
		builder.Where("UserId = @UserId");
		return (await GetRefreshTokensAsync(new { JwtId = jwtId, UserId = userId }, builder)).FirstOrDefault();
	}

	public async Task<RefreshTokenModel?> GetUserRefreshByJwtIdAsync(string jwtId)
	{
		var builder = new SqlBuilder();
		builder.Where("JwtId = @JwtId");
		return (await GetRefreshTokensAsync(new { JwtId = jwtId}, builder)).FirstOrDefault();
	}

	protected async Task<int> UpdateRefreshTokenAsync(RefreshTokenModel refreshToken, SqlBuilder builder)
	{
		var builderTemplate = builder.AddTemplate($@"UPDATE {_refreshTokensTableName} 
						SET UserId = @UserId, JwtId = @JwtId, IsUsed = @IsUsed, 
						IsRevoked = @IsRevoked, CreatedAt = @CreatedAt, ExpiryDate = @ExpiryDate
						/**where**/");
		return await _db.ExecuteAsync(builderTemplate.RawSql, refreshToken);
	}

	protected async Task<IEnumerable<RefreshTokenModel>> GetRefreshTokensAsync(dynamic parameters, SqlBuilder builder)
	{
		var builderTemplate = builder.AddTemplate(
			$@"SELECT * FROM {_refreshTokensTableName} /**where**/");
		return await _db.QueryAsync<RefreshTokenModel, dynamic>(builderTemplate.RawSql, parameters);
	}
}
