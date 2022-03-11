//using Dapper;
//using ZUEPC.DataAccess.Enums;
//using ZUEPC.DataAccess.Models.Users;

//namespace DataAccess.Data.User;
//public partial class UserData : IUserData
//{
//	public async Task<IEnumerable<RoleModel>> GetRolesAsync()
//	{
//		string sql = $"SELECT * FROM {_rolesTableName}";
//		return await _db.QueryAsync<RoleModel, dynamic>(sql, new { });
//	}

//	public async Task<int> InsertUserRoleAsync(int userId, RoleType roleId)
//	{
//		string sql = $@"INSERT INTO {_userRolesTableName} (UserId, RoleId) 
//						VALUES(@UserId, @RoleId)";
//		return await _db.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId });
//	}

//	public async Task<IEnumerable<RoleModel>> GetUserRolesAsync(int id)
//	{
//		var builder = new SqlBuilder();
//		builder.Where("UserId = @UserId");
//		return await GetUserRolesAsync(new { UserId = id }, builder);
//	}

//	public async Task<int> DeleteUserRoleAsync(int userId, RoleType roleId)
//	{
//		var builder = new SqlBuilder();
//		builder.Where("UserId = @UserId");
//		builder.Where("RoleId = @RoleId");
//		return await DeleteUserRoleAsync(new { UserId = userId, RoleId = roleId }, builder);
//	}

//	public async Task<int> DeleteAllUserRolesAsync(int userId)
//	{
//		var builder = new SqlBuilder();
//		builder.Where("UserId = @UserId");
//		return await DeleteUserRoleAsync(new { UserId = userId}, builder);
//	}

//	protected async Task<int> DeleteUserRoleAsync(dynamic parameters, SqlBuilder builder)
//	{
//		var builderTemplate = builder.AddTemplate($@"DELETE FROM { _userRolesTableName} /**where**/");
//		return await _db.ExecuteAsync(builderTemplate.RawSql, parameters);
//	}

//	protected async Task<IEnumerable<RoleModel>> GetUserRolesAsync(dynamic parameters, SqlBuilder builder)
//	{
//		var builderTemplate = builder.AddTemplate($@"SELECT [Id], [Name] FROM {_userRolesTableName}
//													 JOIN {_rolesTableName} ON (RoleId = Id) /**where**/");
//		return await _db.QueryAsync<RoleModel, dynamic>(builderTemplate.RawSql, parameters);
//	}
//}
