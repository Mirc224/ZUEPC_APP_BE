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

	public async Task<RefreshTokenModel?> GetRefreshTokenByTokenAsync(string refreshToken)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(nameof(RefreshTokenModel.Token), refreshToken, builder, parameters);
		return (await GetModelsAsync(parameters, builder)).FirstOrDefault();
	}

	public async Task<RefreshTokenModel?> GetUserRefreshByJwtIdAsync(string jwtId)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(nameof(RefreshTokenModel.JwtId), jwtId, builder, parameters);
		return (await GetModelsAsync(parameters, builder)).FirstOrDefault();
	}

}
