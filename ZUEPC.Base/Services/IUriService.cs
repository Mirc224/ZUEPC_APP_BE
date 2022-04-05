using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Base.Services;

public interface IUriService
{
	public Uri GetPageUri(string route);
	public Uri AddDomainFilterToUri<TDomainFilter>(Uri pageUri, IQueryFilter domainFilter)
		where TDomainFilter : IQueryFilter;
	public Uri AddPaginationToUri(Uri pageUri, PaginationFilter filter);
}
