using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users.InDatabase;

public class SQLRefreshTokenData :
	SQLDbRepositoryBase<RefreshTokenModel>,
	IRefreshTokenData
{
	public SQLRefreshTokenData(ISqlDataAccess db)
		: base(db, TableNameConstants.REFRESH_TOKEN_TABLE, TableAliasConstants.REFRESH_TOKEN_TABLE_ALIAS)
	{
	}

	public async Task<int> DeleteModelByIdAsync(string id)
	{
		return await DeleteModelsWithColumnValueAsync(nameof(RefreshTokenModel.Token), id);
	}

	public async Task<RefreshTokenModel?> GetRefreshTokenByTokenAsync(string refreshToken)
	{
		return (await GetModelsWithColumnValueAsync(nameof(RefreshTokenModel.Token), refreshToken)).FirstOrDefault();
	}

	public async Task<RefreshTokenModel?> GetUserRefreshByJwtIdAsync(string jwtId)
	{
		return (await GetModelsWithColumnValueAsync(nameof(RefreshTokenModel.JwtId), jwtId)).FirstOrDefault();
	}

	public async Task<int> UpdateModelAsync(RefreshTokenModel model)
	{
		SqlBuilder builder = new();
		AddToWhereExpression(nameof(RefreshTokenModel.Token), builder);
		string updateSql = builder.AddTemplate($"{baseUpdateRawSql} /**where**/").RawSql;
		return await db.ExecuteAsync(updateSql, model);
	}
}
