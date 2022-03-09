namespace ZUEPC.DataAccess.Data.Common;

public interface IRepositoryBase<TModel>
{
	Task<TModel?> GetModelByIdAsync(long id);
	Task<long> InsertModelAsync(TModel model);
	Task<int> UpdateModelAsync(TModel model);
	Task<int> DeleteModelByIdAsync(long id);
}
