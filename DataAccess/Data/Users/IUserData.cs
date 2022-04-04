using ZUEPC.DataAccess.Data.Common;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Users;

namespace DataAccess.Data.User;

public interface IUserData : 
	IRepositoryBase<UserModel>,
	IRepositoryWithSimpleIdBase<UserModel, long>,
	IRepositoryWithFilter<UserModel, UserFilter>
{
	Task<UserModel?> GetUserByEmailAsync(string email);
}