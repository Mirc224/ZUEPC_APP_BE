using DataAccess.Data.User;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users;

public class UserInMemoryData : InMemoryBaseRepository<UserModel>, IUserData
{
	public async Task<UserModel?> GetUserByEmailAsync(string email)
	{
		return _repository.Find(x => x.Email == email);
	}

}
