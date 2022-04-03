using ZUEPC.DataAccess.Interfaces;

namespace ZUEPC.DataAccess.Data.Common;

public interface IRepositoryWithSimpleIdBase<TModel, TId>
	where TModel : IItemWithID<TId>
{
	Task<TModel?> GetModelByIdAsync(TId id);
	Task<int> DeleteModelByIdAsync(TId id);
}
