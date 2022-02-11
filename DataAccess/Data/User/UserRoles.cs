using Dapper;
using DataAccess.Enums;
using DataAccess.Models;

namespace DataAccess.Data.User;
public partial class UserData : IUserData
{
	public async Task<IEnumerable<RoleModel>> GetRolesAsync()
	{
		string sql = $"SELECT * FROM {_rolesTableName}";
		return await _db.QueryAsync<RoleModel, dynamic>(sql, new { });
	}

	public async Task<IEnumerable<RoleModel>> GetUserRolesAsync(dynamic parameters, SqlBuilder builder)
	{
		var builderTemplate = builder.AddTemplate($@"SELECT [Id], [Name] FROM {_userRolesTableName}
													 JOIN {_rolesTableName} ON (RoleId = Id) /**where**/");
		return await _db.QueryAsync<RoleModel, dynamic>(builderTemplate.RawSql, parameters);
	}

	public async Task<int> InsertUserRoleAsync(int userId, RolesType roleId)
	{
		string sql = $@"INSERT INTO {_userRolesTableName} (UserId, RoleId) 
						VALUES(@UserId, @RoleId)";
		return await _db.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId });
	}

	public async Task<int> DeleteUserRoleAsync(dynamic parameters, SqlBuilder builder)
	{
		var builderTemplate = builder.AddTemplate($@"DELETE FROM { _userRolesTableName} /**where**/");
		return await _db.ExecuteAsync(builderTemplate.RawSql, parameters);
	}
}
