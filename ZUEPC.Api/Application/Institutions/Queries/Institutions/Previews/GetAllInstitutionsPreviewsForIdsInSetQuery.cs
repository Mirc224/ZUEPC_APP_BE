using MediatR;

namespace ZUEPC.Api.Application.Institutions.Queries.Institutions.Previews;

public class GetAllInstitutionsPreviewsForIdsInSetQuery : IRequest<GetAllInstitutionsPreviewsForIdsInSetQueryResponse>
{
	public IEnumerable<long> InstitutionIds { get; set; }
}
