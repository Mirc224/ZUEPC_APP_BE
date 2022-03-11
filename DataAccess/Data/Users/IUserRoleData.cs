using ZUEPC.Base.Enums.Users;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users;

public interface IUserRoleData : IRepositoryBase<UserRoleModel>
{
	Task<IEnumerable<UserRoleModel>> GetUserRolesByUserIdAsync(long userId);
	Task<UserRoleModel?> GetUserRoleAsync(long userId, long roleId);
	Task<int> DeleteUserRoleByUserIdAsync(long userId);
}
