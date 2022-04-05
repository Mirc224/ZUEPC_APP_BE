using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Base.Queries;

public abstract class PaginatedQueryBase
{
	public PaginationFilter? PaginationFilter { get; set; }
}
