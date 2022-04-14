using MediatR;

namespace ZUEPC.Api.Application.Institutions.Queries.Institutions.Previews.BaseHandlers;

public class GetAllInstitutionPreviewDataForInstitutionIdsInSetQuery : 
	IRequest<GetAllInstitutionPreviewDataForInstitutionIdsInSetQueryResponse>
{
	public IEnumerable<long> InstitutionIds { get; set; }
}
