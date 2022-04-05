using ZUEPC.Common.CQRS.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Base.Queries;

public class PaginatedQueryWithFilterBase<TFilter>
	: PaginatedQueryWithUriBase
	where TFilter : IQueryFilter
{
	public TFilter? QueryFilter { get; set; }
}
