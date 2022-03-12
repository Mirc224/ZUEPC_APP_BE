using ZUEPC.DataAccess.Filters;

namespace ZUEPC.EvidencePublication.Base.Queries;

public abstract class PaginationQueryBase
{
	public PaginationFilter? PaginationFilter { get; set; }
}
