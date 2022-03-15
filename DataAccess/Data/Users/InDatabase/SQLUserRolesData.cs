using Dapper;
using DataAccess.DbAccess;
using System.Dynamic;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users.InDatabase;

public class SQLUserRolesData :
	SQLDbRepositoryBase<UserRoleModel>, IUserRoleData
{
	public SQLUserRolesData(ISqlDataAccess db) 
		: base(db, TableNameConstants.USER_ROLES_TABLE, TableAliasConstants.USER_ROLES_TABLE_ALIAS)
	{
	}

	public async Task<int> DeleteUserRoleByUserIdAsync(long userId)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		AddToWhereExpression(nameof(UserRoleModel.UserId), userId, builder, parameters);
		return await DeleteModelAsync(parameters, builder);
	}

	public async Task<UserRoleModel?> GetUserRoleByUserIdAndRoleIdAsync(long userId, long roleId)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(nameof(UserRoleModel.UserId), userId, builder, parameters);
		AddToWhereExpression(nameof(UserRoleModel.RoleId), roleId, builder, parameters);
		return (await GetModelsAsync(parameters, builder)).FirstOrDefault();
	}

	public async Task<IEnumerable<UserRoleModel>> GetUserRolesByUserIdAsync(long userId)
	{
		SqlBuilder builder = new();
		ExpandoObject parameters = new();
		builder.Select(baseSelect);
		AddToWhereExpression(nameof(UserRoleModel.UserId), userId, builder, parameters);
		return (await GetModelsAsync(new { UserId = userId}, builder));
	}
}
