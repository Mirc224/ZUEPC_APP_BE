using AutoMapper;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.Responses;

namespace ZUEPC.Common.CQRS.CommandHandlers;

public abstract class EPCCreateSimpleModelCommandHandlerBase<TDomain, TModel> : 
	DomainModelHandlerBase<IRepositoryBase<TModel>, TModel>
	where TDomain : EPCDomainBase
	where TModel : EPCModelBase
{
	protected readonly IMapper _mapper;
	public EPCCreateSimpleModelCommandHandlerBase(IMapper mapper, IRepositoryBase<TModel> repository)
		: base(repository)
	{
		_mapper = mapper;
	}

	protected TModel CreateInsertModelFromRequest<TCreateCommand>(TCreateCommand request)
	where TCreateCommand : EPCCreateCommandBase
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
		where TCreateCommand :EPCCreateCommandBase
		where TResponse : ResponseWithDataBase<TDomain>, new()
	{
		TModel insertModel = CreateInsertModelFromRequest(request);

		long insertedId = await _repository.InsertModelAsync(insertModel);
		return CreateSuccessResponseWithDataFromInsertModel<TResponse>(insertModel, insertedId);
	}

	protected TResponse CreateSuccessResponseWithDataFromInsertModel<TResponse>(TModel insertModel, long insertedId)
	where TResponse : ResponseWithDataBase<TDomain>, new()
	{
		TDomain domain = _mapper.Map<TDomain>(insertModel);
		domain.Id = insertedId;
		return new() { Success = true, Data = domain };
	}
}
