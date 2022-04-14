using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Institutions.Queries.Institutions.Previews.BaseHandlers;
using ZUEPC.Base.Helpers;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetAllInstitutionPreviewsQueryHandler :
	EPCInstitutionPreviewQueryHandlerBase,
	IRequestHandler<GetAllInstitutionPreviewsQuery, GetAllInstitutionPreviewsQueryResponse>
{
	public GetAllInstitutionPreviewsQueryHandler(IMapper mapper, IMediator mediator)
		: base(mapper, mediator) { }

	public async Task<GetAllInstitutionPreviewsQueryResponse> Handle(GetAllInstitutionPreviewsQuery request, CancellationToken cancellationToken)
	{
		GetAllInstitutionsQueryResponse response = await _mediator.Send(new GetAllInstitutionsQuery() { 
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
			return PaginationHelper.ProcessResponse<GetAllInstitutionPreviewsQueryResponse, InstitutionPreview, InstitutionFilter>(
			new List<InstitutionPreview>(),
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
		}

		IEnumerable<InstitutionPreview> result = await ProcessInstitutionPreviews(response.Data);
		return PaginationHelper.ProcessResponse<GetAllInstitutionPreviewsQueryResponse, InstitutionPreview, InstitutionFilter>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}
}
