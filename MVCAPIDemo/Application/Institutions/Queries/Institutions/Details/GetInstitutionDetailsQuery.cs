using MediatR;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetInstitutionDetailsQuery : IRequest<GetInstitutionDetailsQueryResponse>
{
	public long InstitutionId { get; set; }
}
