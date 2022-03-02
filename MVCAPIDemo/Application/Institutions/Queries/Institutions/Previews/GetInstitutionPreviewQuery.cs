using MediatR;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetInstitutionPreviewQuery : IRequest<GetInstitutionPreviewQueryResponse>
{
	public long InstitutionId { get; set; }
}
