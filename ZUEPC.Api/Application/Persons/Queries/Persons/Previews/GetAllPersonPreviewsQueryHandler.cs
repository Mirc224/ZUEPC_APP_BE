using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.Persons.Queries.Persons.Previews.BaseHandlers;
using ZUEPC.Base.Helpers;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetAllPersonPreviewsQueryHandler :
	EPCPersonPreviewQueryHandlerBase,
	IRequestHandler<GetAllPersonPreviewsQuery, GetAllPersonPreviewsQueryResponse>
{
	public GetAllPersonPreviewsQueryHandler(IMapper mapper, IMediator mediator)
		: base(mapper, mediator) { }
	
	public async Task<GetAllPersonPreviewsQueryResponse> Handle(GetAllPersonPreviewsQuery request, CancellationToken cancellationToken)
	{
		GetAllPersonsQueryResponse response = await _mediator.Send(new GetAllPersonsQuery() { 
			PaginationFilter = request.PaginationFilter,
			UriService = request.UriService,
			Route = request.Route,
			QueryFilter = request.QueryFilter
		});
		
		if(!response.Success || response.Data is null) 
		{
			return new() { Success = false };
		}
		int totalRecords = response.TotalRecords;

		if (!response.Data.Any())
		{
			return PaginationHelper.ProcessResponse<GetAllPersonPreviewsQueryResponse, PersonPreview, PersonFilter>(
			new List<PersonPreview>(),
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
		}

		IEnumerable<PersonPreview> result = await ProcessPersonPreviews(response.Data);

		return PaginationHelper.ProcessResponse<GetAllPersonPreviewsQueryResponse, PersonPreview, PersonFilter>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}
}
