using ZUEPC.Common.Services.URIServices;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Common.CQRS.Queries;

public abstract class PaginationWithUriQueryBase : PaginationQueryBase
{
	public string? Route { get; set; }
	public IUriService? UriService { get; set; }
}
