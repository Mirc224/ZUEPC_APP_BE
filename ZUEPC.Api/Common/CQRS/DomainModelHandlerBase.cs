using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.Common.CQRS;

public abstract class DomainModelHandlerBase<TRepository, TModel>
{
	protected readonly TRepository _repository;
	public DomainModelHandlerBase(TRepository repository)
	{
		_repository = repository;
	}
}
