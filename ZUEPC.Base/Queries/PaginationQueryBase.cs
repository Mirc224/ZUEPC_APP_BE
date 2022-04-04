using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Base.Queries;

public abstract class PaginationQueryBase
{
	public PaginationFilter? PaginationFilter { get; set; }
}
