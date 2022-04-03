﻿using AutoMapper;
using ZUEPC.Common.CQRS.Queries;
using ZUEPC.Common.Helpers;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.EvidencePublication.Base.Queries;
using ZUEPC.Responses;

namespace ZUEPC.Common.CQRS.QueryHandlers;

public abstract class GetModelsPagedQueryHandlerBase<TRepository,TDomain, TModel, TFilter>:
	DomainModelHandlerBase<TRepository, TModel>
	where TFilter: IQueryFilter
	where TRepository : IRepositoryWithFilter<TModel, TFilter>, IRepositoryBase<TModel>
{
	protected readonly IMapper _mapper;
	public GetModelsPagedQueryHandlerBase(IMapper mapper, TRepository repository)
		: base(repository)
	{
		_mapper = mapper;
	}

	protected async Task<TResponse> ProcessQueryAsync<TQuery, TResponse>(TQuery request)
		where TQuery : PaginationWithUriQueryBase
		where TResponse : PagedResponseBase<IEnumerable<TDomain>>, new()
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
	where TQuery : PaginationQueryBase
	{
		IEnumerable<TModel> result = await GetDataAsync(request.PaginationFilter);
		return _mapper.Map<List<TDomain>>(result);
	}
}
