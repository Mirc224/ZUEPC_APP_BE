using ZUEPC.Common.CQRS.Query;
using ZUEPC.Common.Services.URIServices;
using ZUEPC.DataAccess.Filters;
using ZUEPC.Responses;

namespace ZUEPC.Common.Helpers;

public static class PaginationHelper
{

	public static TResponse ProcessResponse<TResponse, TQuery, TDomain>(
		IEnumerable<TDomain> mappedResult,
		PaginationFilter? paginationFilter,
		IUriService? uriService,
		int totalRecords,
		string? route)
		where TQuery : EPCPaginationQueryWithUriBase
		where TResponse : PagedResponseBase<IEnumerable<TDomain>>, new()
	{
		if (paginationFilter is null)
		{
			return new()
			{
				Success = true,
				Data = mappedResult,
				PageNumber = 1,
				PageSize = mappedResult.Count(),
				TotalRecords = totalRecords
			};
		}
		if (uriService is null)
		{
			return new()
			{
				Success = true,
				Data = mappedResult,
				PageNumber = 1,
				PageSize = mappedResult.Count(),
				TotalRecords = totalRecords
			};
		}

		return CreatePagedReponse<TResponse, TDomain>(
			mappedResult,
			paginationFilter,
			totalRecords,
			uriService,
			route ?? "");
	}

	public static TResponse CreatePagedReponse<TResponse, T>(IEnumerable<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
		where TResponse : PagedResponseBase<IEnumerable<T>>, new()
	{
		TResponse respose = new()
		{
			Data = pagedData,
			TotalRecords = totalRecords,
			PageNumber = validFilter.PageNumber,
			PageSize = validFilter.PageSize,
			Success = true
		};
		double totalPages = ((double)totalRecords / (double)validFilter.PageSize);
		int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
		respose.NextPage =
			validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
			? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
			: null;

		respose.PreviousPage =
			validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
			? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
			: null;
		respose.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
		respose.LastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.PageSize), route);
		respose.TotalPages = roundedTotalPages;
		respose.TotalRecords = totalRecords;
		return respose;
	}
}
