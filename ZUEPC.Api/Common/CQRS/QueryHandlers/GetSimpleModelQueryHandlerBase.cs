using AutoMapper;
using ZUEPC.Responses;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Common.CQRS.QueryHandlers;

public abstract class GetSimpleModelQueryHandlerBase<TDomain, TModel>:
	DomainModelHandlerBase<IRepositoryBase<TModel>, TModel>
	where TDomain : EPCDomainBase
	where TModel : EPCModelBase
{
	protected readonly IMapper _mapper;
	public GetSimpleModelQueryHandlerBase(IMapper mapper, IRepositoryBase<TModel> repository)
		: base(repository)
	{
		_mapper = mapper;
	}

	protected async Task<TResponse> ProcessQueryAsync<TQuery, TResponse>(TQuery request)
		where TQuery : EPCSimpleQueryBase
		where TResponse : ResponseWithDataBase<TDomain>, new()
	{
		TModel? result = await _repository.GetModelByIdAsync(request.Id);
		if (result is null)
		{
			return new() { Success = false };
		}
		TDomain mappedResult = _mapper.Map<TDomain>(result);
		return new() { Success = true, Data = mappedResult };
	}

}
