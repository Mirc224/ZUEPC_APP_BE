using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Application.Institutions.Queries.Institutions.Details.BaseHandlers;
using ZUEPC.Base.Helpers;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetAllInstitutionDetailsQueryHandler :
	EPCInstitutionDetailsQueryHandlerBase,
	IRequestHandler<GetAllInstitutionDetailsQuery, GetAllInstitutionDetailsQueryResponse>
{
	public GetAllInstitutionDetailsQueryHandler(IMapper mapper, IMediator mediator)
		: base(mapper, mediator) { }
	public async Task<GetAllInstitutionDetailsQueryResponse> Handle(GetAllInstitutionDetailsQuery request, CancellationToken cancellationToken)
	{
		GetAllInstitutionsQueryResponse response = await _mediator.Send(new GetAllInstitutionsQuery()
		{
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
			return PaginationHelper.ProcessResponse<GetAllInstitutionDetailsQueryResponse, InstitutionDetails, InstitutionFilter>(
			new List<InstitutionDetails>(),
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
		}

		IEnumerable<InstitutionDetails> result = await ProcessInstitutionDetails(response.Data);

		return PaginationHelper.ProcessResponse<GetAllInstitutionDetailsQueryResponse, InstitutionDetails, InstitutionFilter>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}
}
