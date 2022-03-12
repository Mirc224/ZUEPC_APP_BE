using AutoMapper;
using ZUEPC.Common.CQRS.Query;
using ZUEPC.Common.Helpers;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Filters;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Queries;
using ZUEPC.Responses;

namespace ZUEPC.Common.CQRS.QueryHandlers;

public abstract class GetAllPagedSimpleModelQueryHandlerBase<TDomain, TModel>:
	DomainModelHandlerBase<TModel>
	where TDomain : DomainBase
	where TModel : ModelBase
{
	protected readonly IMapper _mapper;
	public GetAllPagedSimpleModelQueryHandlerBase(IMapper mapper, IRepositoryBase<TModel> repository)
		: base(repository)
	{
		_mapper = mapper;
	}

	protected async Task<TResponse> ProcessQueryAsync<TQuery, TResponse>(TQuery request)
		where TQuery : PaginationQueryWithUriBase
		where TResponse : PagedResponseBase<IEnumerable<TDomain>>, new()
	{
		ICollection<TDomain> mappedResult = await GetMappedDataAsync(request);
		int totalRecords = await _repository.CountAsync();
		return PaginationHelper.ProcessResponse<TResponse, TQuery, TDomain>(
			mappedResult, 
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route);
	}

	protected async Task<ICollection<TDomain>> GetMappedDataAsync<TQuery>(TQuery request)
		where TQuery : PaginationQueryBase
	{
		IEnumerable<TModel> result = await GetData(request.PaginationFilter);
		return _mapper.Map<List<TDomain>>(result);
	}

	protected async Task<IEnumerable<TModel>> GetData(PaginationFilter? filter)
	{
		if (filter is null)
		{
			return await _repository.GetAllAsync();
		}
		return await _repository.GetAllAsync(filter);
	}
}
