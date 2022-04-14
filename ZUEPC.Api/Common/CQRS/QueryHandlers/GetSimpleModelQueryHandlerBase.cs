using AutoMapper;
using ZUEPC.Base.Domain;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Base.Queries;
using ZUEPC.Base.Responses;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Common;

namespace ZUEPC.Common.CQRS.QueryHandlers;

public abstract class GetSimpleModelQueryHandlerBase<TRepository, TDomain, TModel, TId>:
	ModelHandlerBase<TRepository>
	where TRepository : IRepositoryBase<TModel>, IRepositoryWithSimpleIdBase<TModel, TId>
	where TDomain : EPCDomainBase
	where TModel : IItemWithID<TId>
{
	protected readonly IMapper _mapper;
	public GetSimpleModelQueryHandlerBase(IMapper mapper, TRepository repository)
		: base(repository)
	{
		_mapper = mapper;
	}

	protected async Task<TResponse> ProcessQueryAsync<TQuery, TResponse>(TQuery request)
		where TQuery : QueryWithIdBase<TId>
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
