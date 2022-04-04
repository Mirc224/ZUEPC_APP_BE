using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Base.Queries;

public abstract class PaginatedBaseQuery
{
	public PaginationFilter? PaginationFilter { get; set; }
}
