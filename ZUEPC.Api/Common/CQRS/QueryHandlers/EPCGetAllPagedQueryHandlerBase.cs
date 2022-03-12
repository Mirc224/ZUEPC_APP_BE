using ZUEPC.Common.CQRS.Query;
using ZUEPC.Common.Helpers;
using ZUEPC.DataAccess.Data.Common;
using ZUEPC.DataAccess.Models.Common;
using ZUEPC.Responses;

namespace ZUEPC.Common.CQRS.QueryHandlers;

public class EPCGetAllPagedQueryHandlerBase<TModel, TDomain>:
	DomainModelHandlerBase<TModel>
	where TModel : EPCModelBase
{
	public EPCGetAllPagedQueryHandlerBase(IRepositoryBase<TModel> repository)
	: base(repository) {}

}
