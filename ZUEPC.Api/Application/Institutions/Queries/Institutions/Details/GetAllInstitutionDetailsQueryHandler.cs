using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Application.Institutions.Queries.Institutions.Details.BaseHandlers;
using ZUEPC.Common.Extensions;
using ZUEPC.Common.Helpers;
using ZUEPC.DataAccess.Filters;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetAllInstitutionDetailsQueryHandler :
	EPCInstitutionDetailsQueryHandlerBase,
	IRequestHandler<GetAllInstitutionDetailsQuery, GetAllInstitutionDetailsQueryResponse>
{
	public GetAllInstitutionDetailsQueryHandler(IMapper mapper, IMediator mediator)
		: base(mapper, mediator) { }
	public async Task<GetAllInstitutionDetailsQueryResponse> Handle(GetAllInstitutionDetailsQuery request, CancellationToken cancellationToken)
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

		IEnumerable<Institution> institutionDomains = response.Data;
		List<InstitutionDetails> result = new();
		foreach (Institution institution in institutionDomains.OrEmptyIfNull())
		{
			InstitutionDetails institutionDetails = await ProcessInstitutionDetails(institution);
			if (institutionDetails != null)
			{
				result.Add(institutionDetails);
			}
		}
		int totalRecords = response.TotalRecords;
		return PaginationHelper.ProcessResponse<GetAllInstitutionDetailsQueryResponse, InstitutionDetails, InstitutionFilter>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route,
			request.QueryFilter);
	}
}
