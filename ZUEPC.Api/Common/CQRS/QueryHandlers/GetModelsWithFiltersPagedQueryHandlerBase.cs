using AutoMapper;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Common.CQRS.QueryHandlers;
using ZUEPC.Common.Helpers;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.Base.QueryFilters;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.EvidencePublication.Domain.Common;
using ZUEPC.Responses;

namespace ZUEPC.Api.Common.CQRS.QueryHandlers;

public abstract class GetModelsWithFiltersPagedQueryHandlerBase<TRepository, TDomain, TModel, TFilter> :
	GetModelsPagedQueryHandlerBase<TRepository,TDomain, TModel, TFilter>
	where TRepository: IRepositoryWithFilter<TModel, TFilter>, IRepositoryBase<TModel>
	where TFilter : IQueryFilter
{
	protected GetModelsWithFiltersPagedQueryHandlerBase(IMapper mapper, TRepository repository) 
		: base(mapper, repository)
	{
	}

	protected async Task<TResponse> ProcessQueryWithFilterAsync<TQuery, TResponse>(TQuery request)
		where TQuery : PaginationWithFilterQueryBase<TFilter>
		where TResponse : PagedResponseBase<IEnumerable<TDomain>>, new()
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
	where TQuery : PaginationWithFilterQueryBase<TFilter>
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
