using DataAccess.DbAccess;
using DataAccess.Enums;
using DataAccess.Models;
using System.Linq;

namespace DataAccess.Data;

public class UserData : IUserData
{
    private readonly ISqlDataAccess _db;
    private readonly string _userTableName = "[dbo].[User]";
    private readonly string _rolesTableName = "[dbo].[Roles]";
    private readonly string _userRolesTableName = "[dbo].[UserRoles]";

    public UserData(ISqlDataAccess db)
    {
        _db = db;
    }

    public async Task<IEnumerable<UserModel>> GetUsers()
    {
        string sql = $"SELECT * FROM {_userTableName}";
        return await _db.QueryAsync<UserModel, dynamic>(sql, new { });
    }

    public async Task<UserModel?> GetUser(int id)
    {
		string sql = $"SELECT * FROM {_userTableName} WHERE Id = @Id";
		var results = await _db.QueryAsync<UserModel, dynamic>(sql, new { Id = id });
        return results.FirstOrDefault();
    }

    public async Task<UserModel?> GetUserByEmail(string email)
    {
        string sql = $"SELECT * FROM {_userTableName} WHERE Email = @Email";
        var result = await _db.QueryAsync<UserModel, dynamic>(sql, new { Email = email});

        return result.FirstOrDefault();
    }

    public Task<int> InsertUser(UserModel user)
    {
        string sql = $@"INSERT INTO {_userTableName} (FirstName, LastName, Email, PasswordHash, PasswordSalt)
						OUTPUT INSERTED.Id
						VALUES (@FirstName, @LastName, @Email, @PasswordHash, @PasswordSalt)";

        return _db.ExecuteScalarAsync<int, UserModel>(sql, user);
    }

    public Task UpdateUser(UserModel user)
	{
		string sql = $"UPDATE {_userTableName} SET FirstName = @FirstName, LastName = @LastName WHERE Id = @Id";

		return _db.ExecuteAsync(sql, user);
	}

    public Task DeleteUser(int id) =>
        _db.DeleteData("dbo.spUser_Delete", new { Id = id });

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
		return await _db.QueryAsync<RoleModel, dynamic>(sql, new {UserId = id });
	}

	public async Task<int> InsertUserRole(int userId, RolesType roleId)
	{
		string sql = $"INSERT INTO {_userRolesTableName} (UserId, RoleId) " +
			"VALUES(@UserId, @RoleId)";
		return await _db.ExecuteAsync(sql, new {UserId = userId, RoleId = roleId});
	}

	public async Task<int> DeleteUserRole(int userId, RolesType roleId)
	{
		string sql = $"DELETE FROM {_userRolesTableName} WHERE UserId = @UserId, RoleId = @RoleId";
		return await _db.ExecuteAsync(sql, new { UserId = userId, RoleId = roleId });
	}
}
