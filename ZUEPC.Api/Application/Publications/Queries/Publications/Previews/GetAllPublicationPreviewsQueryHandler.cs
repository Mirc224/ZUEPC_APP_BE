using AutoMapper;
using MediatR;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.Publications.Queries.Publications.Previews.BaseHandlers;
using ZUEPC.Base.Helpers;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetAllPublicationPreviewsQueryHandler :
	EPCPublicationPreviewQueryHandlerBase,
	IRequestHandler<GetAllPublicationPreviewsQuery, GetAllPublicationPreviewsQueryResponse>
{
	public GetAllPublicationPreviewsQueryHandler(IMapper mapper, IMediator mediator) 
		: base(mapper, mediator) { }
	
	public async Task<GetAllPublicationPreviewsQueryResponse> Handle(GetAllPublicationPreviewsQuery request, CancellationToken cancellationToken)
	{
		GetAllPublicationsQueryResponse response = await _mediator.Send(new GetAllPublicationsQuery() { 
			PaginationFilter = request.PaginationFilter,
			UriService = request.UriService,
			Route = request.Route,
			QueryFilter = request.QueryFilter
		});

		if (!response.Success || response.Data is null)
		{
			return new() { Success = false };
		}
		int totalRecords = response.TotalRecords;

		if (!response.Data.Any())
		{
			return PaginationHelper.ProcessResponse<GetAllPublicationPreviewsQueryResponse, PublicationPreview, PublicationFilter>(
			new List<PublicationPreview>(),
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
		}

		IEnumerable<PublicationPreview> result = await ProcessPublicationPreviews(response.Data);
		return PaginationHelper.ProcessResponse<GetAllPublicationPreviewsQueryResponse, PublicationPreview, PublicationFilter>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}
}
