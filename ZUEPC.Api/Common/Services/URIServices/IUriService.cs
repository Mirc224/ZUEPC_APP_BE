using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Common.Services.URIServices;

public interface IUriService
{
	public Uri GetPageUri(PaginationFilter filter, string route);
}
