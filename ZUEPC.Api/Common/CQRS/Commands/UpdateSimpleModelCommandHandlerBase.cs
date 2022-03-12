using AutoMapper;
using ZUEPC.Responses;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.Base.Commands;

namespace ZUEPC.Common.CQRS.CommandHandlers;

public abstract class UpdateSimpleModelCommandHandlerBase<TModel> : 
	DomainModelHandlerBase<TModel>
	where TModel : ModelBase
{
	protected readonly IMapper _mapper;
	public UpdateSimpleModelCommandHandlerBase(IMapper mapper, IRepositoryBase<TModel> repository)
		: base(repository)
	{
		_mapper = mapper;
	}

	protected async Task<TResponse> ProcessUpdateCommandFromRequestAsync<TUpdateCommand, TResponse>(TUpdateCommand request)
	where TUpdateCommand : UpdateCommandBase
	where TResponse : ResponseBase, new()
	{
		TModel? currentModel = await _repository.GetModelByIdAsync(request.Id);
		if (currentModel is null)
		{
			return new() { Success = false };
		}
		TModel updatedModel = currentModel;
		_mapper.Map(request, updatedModel);
		int rowsUpdated = await _repository.UpdateModelAsync(updatedModel);
		return new() { Success = rowsUpdated == 1 };
	}
}
