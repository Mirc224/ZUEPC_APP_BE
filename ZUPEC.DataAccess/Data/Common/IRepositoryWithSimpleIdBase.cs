using ZUEPC.Base.ItemInterfaces;

namespace ZUEPC.DataAccess.Data.Common;

public interface IRepositoryWithSimpleIdBase<TModel, TId>
	where TModel : IItemWithID<TId>
{
	Task<TModel?> GetModelByIdAsync(TId id);
	Task<IEnumerable<TModel>> GetModelsWhereIdInSetAsync(IEnumerable<TId> ids);
	Task<int> DeleteModelByIdAsync(TId id);
}
