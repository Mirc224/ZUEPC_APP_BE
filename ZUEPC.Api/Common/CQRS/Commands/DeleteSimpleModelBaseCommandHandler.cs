﻿using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Interfaces;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.Responses;

namespace ZUEPC.Common.CQRS.CommandHandlers;

public abstract class DeleteSimpleModelBaseCommandHandler<TModel, TId>
	: DomainModelHandlerBase<IRepositoryWithSimpleIdBase<TModel, TId>, TModel>
	where TModel : IItemWithID<TId>
{
	public DeleteSimpleModelBaseCommandHandler(IRepositoryWithSimpleIdBase<TModel, TId> repository)
	: base(repository) {}

	protected async Task<TResponse> ProcessDeleteCommandAsync<TDeleteCommand, TResponse>(TDeleteCommand request)
	where TDeleteCommand : DeleteModelCommandBase<TId>
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
