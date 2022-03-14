using Dapper;
using DataAccess.DbAccess;
using ZUEPC.DataAccess.Constants;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users.InDatabase;

public class SQLUserRolesData :
	SQLDbRepositoryBase<UserRoleModel>, IUserRoleData
{
	public SQLUserRolesData(ISqlDataAccess db) 
		: base(db, TableNameConstatnts.USER_ROLES_TABLE, TableAliasConstants.USER_ROLES_TABLE_ALIAS)
	{
	}

	public async Task<int> DeleteUserRoleByUserIdAsync(long userId)
	{
		SqlBuilder builder = new();
		builder.Where("UserId = @UserId");
		return await DeleteModelAsync(new { UserId = userId }, builder);
	}

	public async Task<UserRoleModel?> GetUserRoleByUserIdAndRoleIdAsync(long userId, long roleId)
	{
		SqlBuilder builder = new();
		builder.Select(baseSelect);
		builder.Where("UserId = @UserId");
		builder.Where("RoleId = @RoleId");
		return (await GetModelsAsync(new{ UserId = userId, RoleId = roleId }, builder)).FirstOrDefault();
	}

	public async Task<IEnumerable<UserRoleModel>> GetUserRolesByUserIdAsync(long userId)
	{
		SqlBuilder builder = new();
		builder.Select(baseSelect);
		builder.Where("UserId = @UserId");
		return (await GetModelsAsync(new { UserId = userId}, builder));
	}
}
