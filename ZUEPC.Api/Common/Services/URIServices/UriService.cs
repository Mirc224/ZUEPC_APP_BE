using Microsoft.AspNetCore.WebUtilities;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Common.Services.URIServices;

public class UriService : IUriService
{
	private readonly string _baseUri;
	public UriService(string baseUri)
	{
		_baseUri = baseUri;
	}

	public Uri GetPageUri(PaginationFilter filter, string route)
	{
		Uri _enpointUri = new(string.Concat(_baseUri, route));
		string modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), nameof(filter.PageNumber), filter.PageNumber.ToString());
		modifiedUri = QueryHelpers.AddQueryString(modifiedUri, nameof(filter.PageSize), filter.PageSize.ToString());
		return new Uri(modifiedUri);
	}
}
