using Dapper;
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
		var builder = new SqlBuilder();
		builder.Select("*");
		return await GetUsersAsync(new { }, builder);
	}
	public async Task<UserModel?> GetUserByIdAsync(int userId)
	{
		var builder = new SqlBuilder();
		builder.Select("*");
		builder.Where("Id = @Id");
		return (await GetUsersAsync(new { Id = userId }, builder)).FirstOrDefault();
	}

	public async Task<UserModel?> GetUserByEmailAsync(string email)
	{
		var builder = new SqlBuilder();
		builder.Select("*");
		builder.Where("Email = @Email");
		return (await GetUsersAsync(new { Email = email}, builder)).FirstOrDefault();
	}

	public async Task<int> UpdateUserAsync(UserModel user)
	{
		var builder = new SqlBuilder();
		builder.Where("Id = @Id");
		return await UpdateUserAsync(user, builder);
	}

	public async Task<int> DeleteUserByIdAsync(int id)
	{
		var builder = new SqlBuilder();
		builder.Where("Id = @Id");
		return await DeleteUserAsync(new {Id = id}, builder);
	}

	public Task<int> InsertUserAsync(UserModel user)
    {
        string sql = $@"INSERT INTO {_userTableName} (FirstName, LastName, Email, PasswordHash, PasswordSalt)
						OUTPUT INSERTED.Id
						VALUES (@FirstName, @LastName, @Email, @PasswordHash, @PasswordSalt)";

        return _db.ExecuteScalarAsync<int, UserModel>(sql, user);
    }

    protected async Task<int> DeleteUserAsync(dynamic parameters, SqlBuilder builder)
	{
		var builderTemplate = builder.AddTemplate($"DELETE FROM {_userTableName} /**where**/");
		return await _db.ExecuteAsync(builderTemplate.RawSql, parameters);
	}

	protected async Task<int> UpdateUserAsync(UserModel user, SqlBuilder builder)
	{
		var builderTemplate = builder.AddTemplate($"UPDATE {_userTableName} SET FirstName = @FirstName, LastName = @LastName /**where**/");
		return await _db.ExecuteAsync(builderTemplate.RawSql, user);
	}

	protected async Task<IEnumerable<UserModel>> GetUsersAsync(dynamic parameters, SqlBuilder builder)
	{
		var builderTemplate = builder.AddTemplate($"SELECT /**select**/ FROM {_userTableName} /**where**/");
		return await _db.QueryAsync<UserModel, dynamic>(builderTemplate.RawSql, parameters);
	}

}
