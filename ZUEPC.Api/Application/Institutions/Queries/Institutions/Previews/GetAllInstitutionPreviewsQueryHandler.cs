using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Institutions.Queries.Institutions.Previews.BaseHandlers;
using ZUEPC.Common.Extensions;
using ZUEPC.Common.Helpers;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

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
			Route = request.Route
		});

		if (!response.Success || response.Data is null)
		{
			return new() { Success = false };
		}

		IEnumerable<Institution> institutionDomains = response.Data;
		List<InstitutionPreview> result = new();
		foreach (Institution institution in institutionDomains.OrEmptyIfNull())
		{
			InstitutionPreview InstitutionPreview = await ProcessInstitutionPreview(institution);
			if (InstitutionPreview != null)
			{
				result.Add(InstitutionPreview);
			}
		}
		int totalRecords = response.TotalRecords;
		return PaginationHelper.ProcessResponse<GetAllInstitutionPreviewsQueryResponse, InstitutionPreview>(
			result,
			request.PaginationFilter,
			request.UriService,
			totalRecords,
			request.Route);
	}
}
