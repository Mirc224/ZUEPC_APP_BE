using DataAccess.Enums;
using DataAccess.Models;

namespace DataAccess.Data.User;
public partial class UserData : IUserData
{
	public async Task<IEnumerable<RoleModel>> GetRoles()
	{
		string sql = $"SELECT * FROM {_rolesTableName}";
		return await _db.QueryAsync<RoleModel, dynamic>(sql, new { });
	}

	public async Task<IEnumerable<RoleModel>> GetUserRoles(int id)
	{
		string sql = $@"SELECT [Id], [Name] FROM {_userRolesTableName}
						JOIN {_rolesTableName} ON (RoleId = Id)
						WHERE UserId = @UserId";
		return await _db.QueryAsync<RoleModel, dynamic>(sql, new { UserId = id });
	}

	public async Task<int> InsertUserRole(int userId, RolesType roleId)
	{
		string sql = $"INSERT INTO {_userRolesTableName} (UserId, RoleId) " +
			"VALUES(@UserId, @RoleId)";
		return await _db.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId });
	}

	public async Task<int> DeleteUserRole(int userId, RolesType roleId)
	{
		string sql = $"DELETE FROM {_userRolesTableName} WHERE UserId = @UserId AND RoleId = @RoleId";
		return await _db.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId});
	}
}
