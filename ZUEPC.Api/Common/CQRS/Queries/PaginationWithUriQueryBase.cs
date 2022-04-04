using ZUEPC.Common.Services.URIServices;
using ZUEPC.Base.Queries;

namespace ZUEPC.Common.CQRS.Queries;

public abstract class PaginationWithUriQueryBase : PaginatedBaseQuery
{
	public string? Route { get; set; }
	public IUriService? UriService { get; set; }
}
