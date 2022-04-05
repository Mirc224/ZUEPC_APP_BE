using AutoMapper;
using ZUEPC.Base.Commands;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Base.Responses;

namespace ZUEPC.Common.CQRS.CommandHandlers;

public abstract class UpdateModelWithIdCommandHandlerBase<TRepository, TModel, TId> : 
	DomainModelHandlerBase<TRepository, TModel>
	where TRepository : IRepositoryBase<TModel>, IRepositoryWithSimpleIdBase<TModel, TId> 
	where TModel : IItemWithID<TId>
{
	protected readonly IMapper _mapper;
	public UpdateModelWithIdCommandHandlerBase(IMapper mapper, TRepository repository)
		: base(repository)
	{
		_mapper = mapper;
	}

	protected async Task<TResponse> ProcessUpdateCommandFromRequestAsync<TUpdateCommand, TResponse>(TUpdateCommand request)
	where TUpdateCommand : UpdateCommandWithIdBase<TId>
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
