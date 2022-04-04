using AutoMapper;
using ZUEPC.Base.Commands;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Responses;

namespace ZUEPC.Common.CQRS.CommandHandlers;

public abstract class UpdateSimpleModelCommandHandlerBase<TRepository, TModel, TId> : 
	DomainModelHandlerBase<TRepository, TModel>
	where TRepository : IRepositoryBase<TModel>, IRepositoryWithSimpleIdBase<TModel, TId> 
	where TModel : IItemWithID<TId>
{
	protected readonly IMapper _mapper;
	public UpdateSimpleModelCommandHandlerBase(IMapper mapper, TRepository repository)
		: base(repository)
	{
		_mapper = mapper;
	}

	protected async Task<TResponse> ProcessUpdateCommandFromRequestAsync<TUpdateCommand, TResponse>(TUpdateCommand request)
	where TUpdateCommand : UpdateCommandBase<TId>
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
