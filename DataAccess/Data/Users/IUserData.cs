using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Users;

namespace DataAccess.Data.User;

public interface IUserData : 
	IRepositoryBase<UserModel>,
	IRepositoryWithFilter<UserModel, UserFilter>
{
	Task<UserModel?> GetUserByEmailAsync(string email);
}