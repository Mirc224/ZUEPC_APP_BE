using ZUEPC.Base.Services;
using ZUEPC.Base.Queries;

namespace ZUEPC.Common.CQRS.Queries;

public abstract class PaginatedQueryWithUriBase : PaginatedQueryBase
{
	public string? Route { get; set; }
	public IUriService? UriService { get; set; }
}
