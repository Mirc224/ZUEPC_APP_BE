namespace ZUEPC.Common.CQRS.CommandHandlers;

public abstract class DomainModelHandlerBase<TRepository, TModel>
{
	protected readonly TRepository _repository;
	public DomainModelHandlerBase(TRepository repository)
	{
		_repository = repository;
	}
}
