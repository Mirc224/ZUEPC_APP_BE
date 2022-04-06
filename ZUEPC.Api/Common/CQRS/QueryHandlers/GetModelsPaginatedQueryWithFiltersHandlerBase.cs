using AutoMapper;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;
using ZUEPC.Base.Responses;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.Base.Helpers;
using ZUEPC.DataAccess.Data.Common;

namespace ZUEPC.Api.Common.CQRS.QueryHandlers;

public abstract class GetModelsPaginatedQueryWithFiltersHandlerBase<TRepository, TDomain, TModel, TFilter> :
	GetModelsPaginatedQueryHandlerBase<TRepository,TDomain, TModel, TFilter>
	where TRepository: IRepositoryWithFilter<TModel, TFilter>, IRepositoryBase<TModel>
	where TFilter : IQueryFilter
{
	protected GetModelsPaginatedQueryWithFiltersHandlerBase(IMapper mapper, TRepository repository) 
		: base(mapper, repository)
	{
	}

	protected async Task<TResponse> ProcessQueryWithFilterAsync<TQuery, TResponse>(TQuery request)
		where TQuery : PaginatedQueryWithFilterBase<TFilter>
		where TResponse : PaginatedResponseBase<IEnumerable<TDomain>>, new()
	{
		ICollection<TDomain> mappedResult = await GetMappedDataWithFilterAsync(request);
		int totalRecords = await _repository.CountAsync(request.QueryFilter);
		return PaginationHelper.ProcessResponse<TResponse, TDomain, TFilter>(
			mappedResult,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}

	protected async Task<ICollection<TDomain>> GetMappedDataWithFilterAsync<TQuery>(TQuery request)
	where TQuery : PaginatedQueryWithFilterBase<TFilter>
	{
		IEnumerable<TModel> result = await GetDataWithFilterAsync(request.QueryFilter, request.PaginationFilter);
		return _mapper.Map<List<TDomain>>(result);
	}

	protected virtual async Task<IEnumerable<TModel>> GetDataWithFilterAsync(TFilter? queryFilter, PaginationFilter? filter)
	{
		if (filter is null)
		{
			return await _repository.GetAllAsync();
		}
		if (queryFilter is null)
		{
			return await _repository.GetAllAsync(filter);
		}
		return await _repository.GetAllAsync(queryFilter, filter);
	}
}
