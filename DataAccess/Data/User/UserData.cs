using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data.User;

public partial class UserData : IUserData
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

    public async Task<UserModel?> GetUserById(int id)
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
}
