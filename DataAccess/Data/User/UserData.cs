using Dapper;
using DataAccess.DbAccess;
using DataAccess.Enums;
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

    public async Task<IEnumerable<UserModel>> GetUsersAsync(dynamic parameters, SqlBuilder builder)
    {
		var builderTemplate = builder.AddTemplate($"SELECT /**select**/ FROM {_userTableName} /**where**/");
		return await _db.QueryAsync<UserModel, dynamic>(builderTemplate.RawSql, parameters);
    }

    public Task<int> InsertUserAsync(UserModel user)
    {
        string sql = $@"INSERT INTO {_userTableName} (FirstName, LastName, Email, PasswordHash, PasswordSalt)
						OUTPUT INSERTED.Id
						VALUES (@FirstName, @LastName, @Email, @PasswordHash, @PasswordSalt)";

        return _db.ExecuteScalarAsync<int, UserModel>(sql, user);
    }

    public Task<int> DeleteUserAsync(UserModel user, SqlBuilder builder)
	{
		var builderTemplate = builder.AddTemplate($"DELETE FROM {_userTableName} /**where**/");
		return _db.ExecuteAsync(builderTemplate.RawSql, user);
	}


	public Task<int> UpdateUserAsync(UserModel user, SqlBuilder builder)
	{
		var builderTemplate = builder.AddTemplate($"UPDATE {_userTableName} SET FirstName = @FirstName, LastName = @LastName /**where**/");
		return _db.ExecuteAsync(builderTemplate.RawSql, user);
	}

}
