using AutoMapper;
using ZUEPC.Common.Responses;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.Common.Commands;

public abstract class EPCUpdateSimpleModelCommandHandlerBase<TModel> : 
	EPCSimpleModelCommandHandlerBase<TModel>
	where TModel : EPCBaseModel
{
	protected readonly IMapper _mapper;
	public EPCUpdateSimpleModelCommandHandlerBase(IMapper mapper, IRepositoryBase<TModel> repository)
		: base(repository)
	{
		_mapper = mapper;
	}

	protected async Task<TResponse> ProcessUpdateCommandFromRequestAsync<TUpdateCommand, TResponse>(TUpdateCommand request)
	where TUpdateCommand : EPCUpdateBaseCommand
	where TResponse : ResponseBase, new()
	{
		TModel? currentModel = await _repository.GetModelByIdAsync(request.Id);
		if (currentModel is null)
		{
			return new() { Success = false };
		}
		TModel updatedModel = _mapper.Map<TModel>(request);
		int rowsUpdated = await _repository.UpdateModelAsync(updatedModel);
		return new() { Success = rowsUpdated == 1 };
	}
}
