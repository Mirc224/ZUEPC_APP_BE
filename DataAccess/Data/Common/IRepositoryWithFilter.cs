using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Data.Common;

public interface IRepositoryWithFilter<TModel, TFilter>
	where TModel : ModelBase
	where TFilter : IQueryFilter
{
	public Task<IEnumerable<TModel>> GetAllAsync(TFilter queryFilter, PaginationFilter paginationFilter);
	public Task<int> CountAsync(TFilter queryFilter);
}
