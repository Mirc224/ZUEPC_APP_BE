using ZUEPC.Common.Services.URIServices;
using ZUEPC.EvidencePublication.Base.Queries;

namespace ZUEPC.Common.CQRS.Query;

public class EPCPaginationQueryWithUriBase 
	: EPCPaginationQueryBase
{
	public string? Route { get; set; }
	public IUriService? UriService { get; set; }
}
