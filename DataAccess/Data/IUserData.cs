using DataAccess.Enums;
using DataAccess.Models;

namespace DataAccess.Data;

public interface IUserData
{
    Task DeleteUser(int id);
    Task<UserModel?> GetUser(int id);
    Task<UserModel?> GetUserByEmail(string email);
    Task<IEnumerable<UserModel>> GetUsers();
    Task<int> InsertUser(UserModel user);
    Task UpdateUser(UserModel user);
	Task<IEnumerable<RoleModel>> GetRoles();
	Task<IEnumerable<RoleModel>> GetUserRoles(int id);
	Task<int> InsertUserRole(int userId, RolesType roleId);
	Task<int> DeleteUserRole(int userId, RolesType roleId);
}