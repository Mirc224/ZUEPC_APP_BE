using AutoMapper;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;
using ZUEPC.Base.Responses;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.Common.CQRS.Queries;
using ZUEPC.Base.Helpers;
using ZUEPC.DataAccess.Data.Common;

namespace ZUEPC.Common.CQRS.QueryHandlers;

public abstract class GetModelsPaginatedQueryHandlerBase<TRepository, TDomain, TModel, TFilter>:
	ModelHandlerBase<TRepository>
	where TFilter: IQueryFilter
	where TRepository : IRepositoryWithFilter<TModel, TFilter>, IRepositoryBase<TModel>
{
	protected readonly IMapper _mapper;
	public GetModelsPaginatedQueryHandlerBase(IMapper mapper, TRepository repository)
		: base(repository)
	{
		_mapper = mapper;
	}

	protected async Task<TResponse> ProcessQueryAsync<TQuery, TResponse>(TQuery request)
		where TQuery : PaginatedQueryWithUriBase
		where TResponse : PaginatedResponseBase<IEnumerable<TDomain>>, new()
	{
		ICollection<TDomain> mappedResult = await GetMappedDataAsync(request);
		int totalRecords = await _repository.CountAsync();
		return PaginationHelper.ProcessResponse<TResponse, TDomain>(
			mappedResult, 
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route);
	}

	protected virtual async Task<IEnumerable<TModel>> GetDataAsync(PaginationFilter? filter)
	{
		if (filter is null)
		{
			return await _repository.GetAllAsync();
		}
		return await _repository.GetAllAsync(filter);
	}

	protected async Task<ICollection<TDomain>> GetMappedDataAsync<TQuery>(TQuery request)
	where TQuery : PaginatedQueryBase
	{
		IEnumerable<TModel> result = await GetDataAsync(request.PaginationFilter);
		return _mapper.Map<List<TDomain>>(result);
	}
}
