﻿using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users;

public interface IUserRoleData : 
	IRepositoryBase<UserRoleModel>
{
	Task<IEnumerable<UserRoleModel>> GetUserRolesByUserIdAsync(long userId);
	Task<UserRoleModel?> GetUserRoleByUserIdAndRoleIdAsync(long userId, long roleId);
	Task<int> DeleteUserRoleByUserIdAsync(long userId);
	Task<int> DeleteUserRoleByUserIdAndRoleIdAsync(long userId, long roleId);
}
