using ZUEPC.DataAccess.Filters;

namespace ZUEPC.EvidencePublication.Base.Queries;

public abstract class EPCPaginationQueryBase
{
	public PaginationFilter? PaginationFilter { get; set; }
}
