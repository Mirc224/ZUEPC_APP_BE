using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.Common.Commands;

public abstract class EPCSimpleModelCommandHandlerBase<TModel>
	where TModel : EPCBaseModel
{
	protected readonly IRepositoryBase<TModel> _repository;
	public EPCSimpleModelCommandHandlerBase(IRepositoryBase<TModel> repository)
	{
		_repository = repository;
	}
}
