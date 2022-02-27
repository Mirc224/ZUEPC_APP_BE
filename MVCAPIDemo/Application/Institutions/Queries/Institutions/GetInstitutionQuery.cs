using MediatR;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetInstitutionQuery : IRequest<GetInstitutionQueryResponse>
{
	public long InstitutionId { get; set; }
}
