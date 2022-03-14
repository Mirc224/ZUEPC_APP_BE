using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Common.Services.URIServices;

public interface IUriService
{
	public Uri GetPageUri(string route);
	public Uri AddDomainFilterToUri<TDomainFilter>(Uri pageUri, IQueryFilter domainFilter)
		where TDomainFilter : IQueryFilter;
	public Uri AddPaginationToUri(Uri pageUri, PaginationFilter filter);
}
