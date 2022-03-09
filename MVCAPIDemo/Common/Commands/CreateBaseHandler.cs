using AutoMapper;
using ZUEPC.Common.Responses;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.EvidencePublication.Base.Commands;
using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.Common.Commands;

public abstract class CreateBaseHandler
{
	protected IMapper _mapper;

	protected TModel CreateInsertModelFromRequest<TModel, TCreateCommand>(TCreateCommand request)
	where TCreateCommand : EPCCreateBaseCommand
	where TModel : EPCBaseModel
	{
		TModel insertModel = _mapper.Map<TModel>(request);
		insertModel.CreatedAt = DateTime.UtcNow;
		if (request.VersionDate is null)
		{
			insertModel.CreatedAt = DateTime.UtcNow;
		}
		return insertModel;
	}

	protected TResponse CreateSuccessResponseWithDataFromInsertModel<TResponse, TDomain, TModel>(TModel insertModel, long insertedId)
	where TResponse : ResponseBaseWithData<TDomain>, new()
	where TDomain : EPCBase
	where TModel : EPCBaseModel
	{
		TDomain domain = _mapper.Map<TDomain>(insertModel);
		domain.Id = insertedId;
		return new() { Success = true, Data = domain };
	}
}
