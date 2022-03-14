using Microsoft.AspNetCore.WebUtilities;
using System.Collections;
using System.Reflection;
using ZUEPC.DataAccess.Filters;

namespace ZUEPC.Common.Services.URIServices;

public class UriService : IUriService
{
	private readonly string _baseUri;
	public UriService(string baseUri)
	{
		_baseUri = baseUri;
	}

	public Uri AddDomainFilterToUri<TDomainFilter>(Uri pageUri, IQueryFilter domainFilter)
		where TDomainFilter : IQueryFilter
	{
		string modifiedUri = pageUri.ToString();
		Type filterType = domainFilter.GetType();
		IList<PropertyInfo> props = new List<PropertyInfo>(filterType.GetProperties());
		foreach (PropertyInfo prop in props)
		{
			object? propValue = prop.GetValue(domainFilter, null);
			if (propValue is null)
			{
				continue;
			}
			if (typeof(ICollection).IsAssignableFrom(prop.PropertyType))
			{
				foreach(object arrayItem in (ICollection)propValue)
				{
					modifiedUri = QueryHelpers.AddQueryString(modifiedUri, prop.Name, arrayItem.ToString());
				}
				continue;
			}
			modifiedUri = QueryHelpers.AddQueryString(modifiedUri, prop.Name, propValue.ToString());
		}
		return new Uri(modifiedUri);
	}

	public Uri AddPaginationToUri(Uri pageUri, PaginationFilter filter)
	{
		string modifiedUri = QueryHelpers.AddQueryString(pageUri.ToString(), nameof(filter.PageNumber), filter.PageNumber.ToString());
		modifiedUri = QueryHelpers.AddQueryString(modifiedUri, nameof(filter.PageSize), filter.PageSize.ToString());
		return new Uri(modifiedUri);
	}

	public Uri GetPageUri(PaginationFilter filter, string route)
	{
		Uri _enpointUri = new(string.Concat(_baseUri, route));
		string modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), nameof(filter.PageNumber), filter.PageNumber.ToString());
		modifiedUri = QueryHelpers.AddQueryString(modifiedUri, nameof(filter.PageSize), filter.PageSize.ToString());
		return new Uri(modifiedUri);
	}

	public Uri GetPageUri(string route)
	{
		return new(string.Concat(_baseUri, route));
	}
}
