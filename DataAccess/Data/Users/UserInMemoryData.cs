using DataAccess.Data.User;
using System.Linq.Expressions;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Users;

namespace ZUEPC.DataAccess.Data.Users;

public class UserInMemoryData : InMemoryBaseRepository<UserModel>, IUserData
{
	public Task<int> CountAsync(UserFilter queryFilter)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<UserModel>> GetAllAsync(UserFilter userFilter, PaginationFilter paginationFilter) 
	{
		Expression? expression = GetFilterExpression(userFilter);
		if(expression is null)
		{
			return await GetAllAsync(paginationFilter);
		}
		var condition = Expression.Lambda<Func<UserModel, bool>>(expression, Expression.Parameter(typeof(UserModel))).Compile();
		return _repository.Where(condition);
	}

	public Task<IEnumerable<UserModel>> GetAllAsync<TFilter>(TFilter queryFilter, PaginationFilter paginationFilter) where TFilter : IQueryFilter
	{
		throw new NotImplementedException();
	}

	public async Task<UserModel?> GetUserByEmailAsync(string email)
	{
		return _repository.Find(x => x.Email == email);
	}
}
