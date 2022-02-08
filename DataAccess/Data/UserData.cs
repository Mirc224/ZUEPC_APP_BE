using DataAccess.DbAccess;
using DataAccess.Models;
using System.Linq;

namespace DataAccess.Data;

public class UserData : IUserData
{
    private readonly ISqlDataAccess _db;
    private readonly string _name = "[dbo].[User]";

    public UserData(ISqlDataAccess db)
    {
        _db = db;
    }

    //public Task<IEnumerable<UserModel>> GetUsers() =>
    //     _db.LoadData<UserModel, dynamic>("dbo.spUser_GetAll", new { });
    public async Task<IEnumerable<UserModel>> GetUsers()
    {
        string sql = $"SELECT * FROM {_name}";
        return await _db.QueryAsync<UserModel, dynamic>(sql, new { });
    }

    public async Task<UserModel?> GetUser(int id)
    {
        var results = await _db.LoadData<UserModel, dynamic>(
            "dbo.spUser_Get",
            new { Id = id });

        return results.FirstOrDefault();
    }

    public async Task<UserModel?> GetUserByEmail(string email)
    {
        string sql = $"SELECT * FROM {_name} WHERE Email = @Email";
        var result = await _db.QueryAsync<UserModel, dynamic>(sql, new { Email = email});

        return result.FirstOrDefault();
    }

    //public Task InsertUser(UserModel user) =>
    //    _db.SaveData("dbo.spUser_Insert", new { user.FirstName, user.LastName });
    public Task<int> InsertUser(UserModel user)
    {
        string sql = $"INSERT INTO {_name} (FirstName, LastName, Email, PasswordHash, PasswordSalt) " +
            "VALUES(@FirstName, @LastName, @Email, @PasswordHash, @PasswordSalt)";

        return _db.ExecuteAsync(sql, user);
    }

    public Task UpdateUser(UserModel user) =>
        _db.UpdateData("dbo.spUser_Insert", user);

    public Task DeleteUser(int id) =>
        _db.DeleteData("dbo.spUser_Delete", new { Id = id });
}
