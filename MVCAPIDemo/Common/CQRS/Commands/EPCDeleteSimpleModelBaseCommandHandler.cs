﻿using ZUEPC.Responses;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Common.CQRS.CommandHandlers;

public abstract class EPCDeleteSimpleModelBaseCommandHandler<TModel>
	: EPCDomainModelHandlerBase<TModel>
	where TModel : EPCBaseModel
{
	public EPCDeleteSimpleModelBaseCommandHandler(IRepositoryBase<TModel> repository)
	: base(repository) {}

	protected async Task<TResponse> ProcessDeleteCommandAsync<TDeleteCommand, TResponse>(TDeleteCommand request)
	where TDeleteCommand : EPCDeleteCommandBase
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
