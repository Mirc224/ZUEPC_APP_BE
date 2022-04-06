namespace ZUEPC.Common.CQRS.CommandHandlers;

public abstract class ModelHandlerBase<TRepository>
{
	protected readonly TRepository _repository;
	public ModelHandlerBase(TRepository repository)
	{
		_repository = repository;
	}
}
