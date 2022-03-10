using ZUEPC.DataAccess.Filters;

namespace ZUEPC.DataAccess.Data.Common;

public interface IRepositoryBase<TModel>
{
	Task<TModel?> GetModelByIdAsync(long id);
	Task<IEnumerable<TModel>> GetAllAsync();
	Task<IEnumerable<TModel>> GetAllAsync(PaginationFilter filter);
	Task<long> InsertModelAsync(TModel model);
	Task<int> UpdateModelAsync(TModel model);
	Task<int> DeleteModelByIdAsync(long id);
	Task<int> CountAsync();
}
