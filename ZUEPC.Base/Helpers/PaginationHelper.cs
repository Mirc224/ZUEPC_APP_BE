using ZUEPC.Base.Services;
using ZUEPC.Base.QueryFilters;
using ZUEPC.Base.Responses;

namespace ZUEPC.Base.Helpers;

public static class PaginationHelper
{

	public static TResponse ProcessResponse<TResponse, TDomain, TModelFilter>(
		IEnumerable<TDomain> pagedData,
		PaginationFilter? paginationFilter,
		IUriService uriService,
		int totalRecords,
		string route,
		TModelFilter? modelFilter)
		where TResponse : PaginatedResponseBase<IEnumerable<TDomain>>, new()
		where TModelFilter: IQueryFilter
	{
		Uri pageUri = uriService.GetPageUri(route);
		if(modelFilter != null)
		{
			pageUri = uriService.AddDomainFilterToUri<TModelFilter>(pageUri, modelFilter);
		}
		return CreatePagedReponse<TResponse, TDomain>(pageUri, pagedData, paginationFilter, uriService, totalRecords);
	}

	public static TResponse ProcessResponse<TResponse, TDomain>(
		IEnumerable<TDomain> pagedData,
		PaginationFilter paginationFilter,
		IUriService uriService,
		int totalRecords,
		string route)
		where TResponse : PaginatedResponseBase<IEnumerable<TDomain>>, new()
	{
		Uri pageUri = uriService.GetPageUri(route);
		return CreatePagedReponse<TResponse, TDomain>(pageUri, pagedData, paginationFilter, uriService, totalRecords);
	}

	public static TResponse CreatePagedReponse<TResponse, TDomain>(
		Uri pageUri,
		IEnumerable<TDomain> pagedData, 
		PaginationFilter paginationFilter,  
		IUriService uriService,
		int totalRecords)
		where TResponse : PaginatedResponseBase<IEnumerable<TDomain>>, new()
	{
		TResponse response = new()
		{
			Data = pagedData,
			TotalRecords = totalRecords,
			PageNumber = paginationFilter.PageNumber,
			PageSize = paginationFilter.PageSize,
			Success = true
		};
		response = AddPaginationToUri<TResponse, TDomain>(pageUri, response, paginationFilter, uriService, totalRecords);
		return response;
	}

	public static TResponse AddPaginationToUri<TResponse, TDomain>(
		Uri pageUri, 
		TResponse response,
		PaginationFilter paginationFilter,
		IUriService uriService,
		int totalRecords)
		where TResponse : PaginatedResponseBase<IEnumerable<TDomain>>, new()
	{
		double totalPages = ((double)totalRecords / (double)paginationFilter.PageSize);
		int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
		response.NextPage =
			paginationFilter.PageNumber >= 1 && paginationFilter.PageNumber < roundedTotalPages
			? uriService.AddPaginationToUri(pageUri,
			new PaginationFilter()
			{
				PageNumber = paginationFilter.PageNumber + 1,
				PageSize = paginationFilter.PageSize
			})
			: null;

		response.PreviousPage =
			paginationFilter.PageNumber - 1 >= 1 && paginationFilter.PageNumber <= roundedTotalPages
			? uriService.AddPaginationToUri(pageUri,
			new PaginationFilter()
			{
				PageNumber = paginationFilter.PageNumber - 1,
				PageSize = paginationFilter.PageSize
			})
			: null;
		response.FirstPage = uriService.AddPaginationToUri(pageUri,
		new PaginationFilter()
		{
			PageNumber = 1,
			PageSize = paginationFilter.PageSize
		});
		response.LastPage = uriService.AddPaginationToUri(pageUri,
		new PaginationFilter()
		{
			PageNumber = roundedTotalPages,
			PageSize = paginationFilter.PageSize
		});
		response.TotalPages = roundedTotalPages;
		response.TotalRecords = totalRecords;
		return response;
	}
}
