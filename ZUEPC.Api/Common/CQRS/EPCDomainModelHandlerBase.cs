using ZUEPC.DataAccess.Data.Common;

namespace ZUEPC.Common.CQRS;

public abstract class EPCDomainModelHandlerBase<TRepository, TModel>
{
	protected readonly TRepository _repository;
	public EPCDomainModelHandlerBase(TRepository repository)
	{
		_repository = repository;
	}
}
