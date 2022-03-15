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
		return (await GetModelsWithColumnValue(nameof(RefreshTokenModel.Token), refreshToken)).FirstOrDefault();
	}

	public async Task<RefreshTokenModel?> GetUserRefreshByJwtIdAsync(string jwtId)
	{
		return (await GetModelsWithColumnValue(nameof(RefreshTokenModel.JwtId), jwtId)).FirstOrDefault();
	}

}
