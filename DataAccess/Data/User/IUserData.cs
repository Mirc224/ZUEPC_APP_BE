using DataAccess.Enums;
using DataAccess.Models;

namespace DataAccess.Data.User;

public interface IUserData
{
    Task<int> DeleteUserAsync(int id);
    Task<UserModel?> GetUserByIdAsync(int id);
    Task<UserModel?> GetUserByEmailAsync(string email);
    Task<IEnumerable<UserModel>> GetUsersAsync();
    Task<int> InsertUserAsync(UserModel user);
    Task<int> UpdateUserAsync(UserModel user);
	Task<IEnumerable<RoleModel>> GetRolesAsync();
	Task<IEnumerable<RoleModel>> GetUserRolesAsync(int id);
	Task<int> InsertUserRoleAsync(int userId, RolesType roleId);
	Task<int> DeleteUserRoleAsync(int userId, RolesType roleId);
	Task<int> InsertRefreshTokenAsync(RefreshTokenModel refreshToken);
	Task<RefreshTokenModel?> GetRefreshTokenByTokenAsync(string refreshToken);
	Task<int> UpdateRefreshTokenAsync(RefreshTokenModel refreshTokenModel);
}