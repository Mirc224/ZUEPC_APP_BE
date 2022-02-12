﻿using Dapper;
using DataAccess.Enums;
using DataAccess.Models;

namespace DataAccess.Data.User;

public interface IUserData
{
    Task<int> DeleteUserByIdAsync(int id);
    Task<IEnumerable<UserModel>> GetUsersAsync();
    Task<UserModel?> GetUserByIdAsync(int userId);
	Task<UserModel?> GetUserByEmailAsync(string email);
	Task<int> InsertUserAsync(UserModel user);
    Task<int> UpdateUserAsync(UserModel user);
	Task<IEnumerable<RoleModel>> GetRolesAsync();
	Task<IEnumerable<RoleModel>> GetUserRolesAsync(int id);
	Task<int> InsertUserRoleAsync(int userId, RolesType roleId);
	Task<int> DeleteUserRoleAsync(int userId, RolesType roleId);
	Task<int> DeleteAllUserRolesAsync(int userId);
	Task<Guid> InsertRefreshTokenAsync(RefreshTokenModel refreshToken);
	Task<RefreshTokenModel?> GetRefreshTokenByTokenAsync(string refreshToken);
	Task<int> UpdateRefreshTokenAsync(RefreshTokenModel refreshTokenModel);
	Task<RefreshTokenModel?> GetUserRefreshTokenAsync(int userId, string jwtId);
	Task<RefreshTokenModel?> GetUserRefreshByJwtIdAsync(string jwtId);
}