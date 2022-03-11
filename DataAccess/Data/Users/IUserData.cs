using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Users;

namespace DataAccess.Data.User;

public interface IUserData 
	: IRepositoryBase<UserModel>
{
	Task<UserModel?> GetUserByEmailAsync(string email);
	
}