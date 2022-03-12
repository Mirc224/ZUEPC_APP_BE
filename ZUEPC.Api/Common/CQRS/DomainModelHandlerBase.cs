using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.Common.CQRS;

public abstract class DomainModelHandlerBase<TModel>
	where TModel : ModelBase
{
	protected readonly IRepositoryBase<TModel> _repository;
	public DomainModelHandlerBase(IRepositoryBase<TModel> repository)
	{
		_repository = repository;
	}
}
