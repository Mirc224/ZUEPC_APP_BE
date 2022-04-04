using ZUEPC.Base.QueryFilters;

namespace ZUEPC.DataAccess.Data.Common;

public interface IRepositoryBase<TModel>
{
	Task<IEnumerable<TModel>> GetAllAsync();
	Task<long> InsertModelAsync(TModel model);
	Task<int> UpdateModelAsync(TModel model);
	Task<int> CountAsync();
}
