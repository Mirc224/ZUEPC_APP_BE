//using AutoMapper;
//using ZUEPC.Base.Responses;
//using ZUEPC.DataAccess.Data.Common;
//using ZUEPC.DataAccess.Models.Common;
//using ZUEPC.Base.Domain;
//using ZUEPC.Base.Queries;

//namespace ZUEPC.Common.CQRS.QueryHandlers;

//public abstract class GetEPCSimpleModelQueryHandlerBase<TDomain, TModel> :
//	EPCDomainModelHandlerBase<IRepositoryWithSimpleIdBase<TModel>, TModel>
//	where TDomain : EPCDomainBase
//	where TModel : EPCModelBase
//{
//	protected readonly IMapper _mapper;
//	public GetEPCSimpleModelQueryHandlerBase(IMapper mapper, IRepositoryWithSimpleIdBase<TModel> repository)
//		: base(repository)
//	{
//		_mapper = mapper;
//	}

//	protected async Task<TResponse> ProcessQueryAsync<TQuery, TResponse>(TQuery request)
//		where TQuery : EPCSimpleQueryBase
//		where TResponse : ResponseWithDataBase<TDomain>, new()
//	{
//		TModel? result = await _repository.GetModelByIdAsync(request.Id);
//		if (result is null)
//		{
//			return new() { Success = false };
//		}
//		TDomain mappedResult = _mapper.Map<TDomain>(result);
//		return new() { Success = true, Data = mappedResult };
//	}

//}
