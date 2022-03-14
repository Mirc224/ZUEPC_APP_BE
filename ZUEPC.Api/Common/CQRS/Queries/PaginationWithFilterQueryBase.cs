using ZUEPC.Common.CQRS.Queries;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Api.Common.CQRS.Queries;

public class PaginationWithFilterQueryBase<TFilter>
	: PaginationWithUriQueryBase
	where TFilter : IQueryFilter
{
	public TFilter? QueryFilter { get; set; }
}
