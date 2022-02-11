using Dapper;
using DataAccess.Enums;
using DataAccess.Models;

namespace DataAccess.Data.User;

public interface IUserData
{
    Task<int> DeleteUserAsync(UserModel user, SqlBuilder builder);
    Task<IEnumerable<UserModel>> GetUsersAsync(dynamic parameters, SqlBuilder builder);
    Task<int> InsertUserAsync(UserModel user);
    Task<int> UpdateUserAsync(UserModel user, SqlBuilder builder);
	Task<IEnumerable<RoleModel>> GetRolesAsync();
	Task<IEnumerable<RoleModel>> GetUserRolesAsync(dynamic parameters, SqlBuilder builder);
	Task<int> InsertUserRoleAsync(int userId, RolesType roleId);
	Task<int> DeleteUserRoleAsync(dynamic parameters, SqlBuilder builder);
	Task<Guid> InsertRefreshTokenAsync(RefreshTokenModel refreshToken);
	Task<RefreshTokenModel?> GetRefreshTokenByTokenAsync(string refreshToken);
	Task<int> UpdateRefreshTokenAsync(RefreshTokenModel refreshTokenModel);
	Task<RefreshTokenModel?> GetRefreshTokenByJwtIdAsync(string jwtId);
}