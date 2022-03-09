using AutoMapper;
using ZUEPC.Common.Responses;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.Common.Commands;

public abstract class EPCCreateSimpleModelCommandHandlerBase<TDomain, TModel> 
	: EPCSimpleModelCommandHandlerBase<TModel>
	where TDomain : EPCBase
	where TModel : EPCBaseModel
{
	protected readonly IMapper _mapper;
	public EPCCreateSimpleModelCommandHandlerBase(IMapper mapper, IRepositoryBase<TModel> repository)
		: base(repository)
	{
		_mapper = mapper;
	}

	protected TModel CreateInsertModelFromRequest<TCreateCommand>(TCreateCommand request)
	where TCreateCommand : EPCCreateBaseCommand
	{
		TModel insertModel = _mapper.Map<TModel>(request);
		insertModel.CreatedAt = DateTime.UtcNow;
		if (request.VersionDate is null)
		{
			insertModel.CreatedAt = DateTime.UtcNow;
		}
		return insertModel;
	}

	protected async Task<TResponse> ProcessInsertCommandAsync<TCreateCommand, TResponse>(TCreateCommand request)
		where TCreateCommand :EPCCreateBaseCommand
		where TResponse : ResponseBaseWithData<TDomain>, new()
	{
		TModel insertModel = CreateInsertModelFromRequest(request);

		long insertedId = await _repository.InsertModelAsync(insertModel);
		return CreateSuccessResponseWithDataFromInsertModel<TResponse>(insertModel, insertedId);
	}

	protected TResponse CreateSuccessResponseWithDataFromInsertModel<TResponse>(TModel insertModel, long insertedId)
	where TResponse : ResponseBaseWithData<TDomain>, new()
	{
		TDomain domain = _mapper.Map<TDomain>(insertModel);
		domain.Id = insertedId;
		return new() { Success = true, Data = domain };
	}
}
