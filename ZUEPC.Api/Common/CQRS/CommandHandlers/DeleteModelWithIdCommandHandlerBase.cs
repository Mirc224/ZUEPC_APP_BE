using ZUEPC.DataAccess.Data.Common;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Base.Commands;
using ZUEPC.Base.Responses;

namespace ZUEPC.Common.CQRS.CommandHandlers;

public abstract class DeleteModelWithIdCommandHandlerBase<TModel, TId>
	: DomainModelHandlerBase<IRepositoryWithSimpleIdBase<TModel, TId>, TModel>
	where TModel : IItemWithID<TId>
{
	public DeleteModelWithIdCommandHandlerBase(IRepositoryWithSimpleIdBase<TModel, TId> repository)
	: base(repository) {}

	protected async Task<TResponse> ProcessDeleteCommandAsync<TDeleteCommand, TResponse>(TDeleteCommand request)
	where TDeleteCommand : EPCDeleteModelCommandBase<TId>
	where TResponse : ResponseBase, new()
	{
		TModel? currentModel = await _repository.GetModelByIdAsync(request.Id);
		if(currentModel is null) {
			return new() { Success = false };
		}
		int rowsDeleted = await _repository.DeleteModelByIdAsync(request.Id);
		return new() { Success = true};
	}
}
