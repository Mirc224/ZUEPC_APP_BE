using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.Common.CQRS;

public abstract class EPCDomainModelHandlerBase<TModel>
	where TModel : EPCBaseModel
{
	protected readonly IRepositoryBase<TModel> _repository;
	public EPCDomainModelHandlerBase(IRepositoryBase<TModel> repository)
	{
		_repository = repository;
	}
}
