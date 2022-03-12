using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users;

public class UserRoleInMemoryData : InMemoryBaseRepository<UserRoleModel>, IUserRoleData
{

	public async Task<int> DeleteUserRoleByUserIdAsync(long userId)
	{
		var deletedObjects = _repository.Where(x => x.UserId == userId).ToList();
		return await DeleteRecordsAsync(deletedObjects);
	}

	public async Task<UserRoleModel?> GetUserRoleByUserIdAndRoleIdAsync(long userId, long roleId)
	{
		return _repository.Find(x => x.UserId == userId && x.RoleId == roleId);
	}

	public async Task<IEnumerable<UserRoleModel>> GetUserRolesByUserIdAsync(long userId)
	{
		return _repository.Where(x => x.UserId == userId);
	}
}
