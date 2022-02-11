using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Data.User;

public partial class UserData : IUserData
{
    private readonly ISqlDataAccess _db;
    private readonly string _userTableName = "[dbo].[Users]";
    private readonly string _rolesTableName = "[dbo].[Roles]";
    private readonly string _userRolesTableName = "[dbo].[UserRoles]";
    private readonly string _refreshTokensTableName = "[dbo].[RefreshTokens]";

    public UserData(ISqlDataAccess db)
    {
        _db = db;
    }

    public async Task<IEnumerable<UserModel>> GetUsersAsync()
    {
        string sql = $"SELECT * FROM {_userTableName}";
        return await _db.QueryAsync<UserModel, dynamic>(sql, new { });
    }

    public async Task<UserModel?> GetUserByIdAsync(int id)
    {
		string sql = $"SELECT * FROM {_userTableName} WHERE Id = @Id";
		var results = await _db.QueryAsync<UserModel, dynamic>(sql, new { Id = id });
        return results.FirstOrDefault();
    }

    public async Task<UserModel?> GetUserByEmailAsync(string email)
    {
        string sql = $"SELECT * FROM {_userTableName} WHERE Email = @Email";
        var result = await _db.QueryAsync<UserModel, dynamic>(sql, new { Email = email});

        return result.FirstOrDefault();
    }

    public Task<int> InsertUserAsync(UserModel user)
    {
        string sql = $@"INSERT INTO {_userTableName} (FirstName, LastName, Email, PasswordHash, PasswordSalt)
						OUTPUT INSERTED.Id
						VALUES (@FirstName, @LastName, @Email, @PasswordHash, @PasswordSalt)";

        return _db.ExecuteScalarAsync<int, UserModel>(sql, user);
    }

    public Task<int> UpdateUserAsync(UserModel user)
	{
		string sql = $"UPDATE {_userTableName} SET FirstName = @FirstName, LastName = @LastName WHERE Id = @Id";

		return _db.ExecuteAsync(sql, user);
	}

    public Task<int> DeleteUserAsync(int id)
	{
		string sql = $@"DELETE FROM {_userTableName} 
						WHERE Id = @Id";
		return _db.ExecuteAsync(sql, new { Id = id });
	}
}
