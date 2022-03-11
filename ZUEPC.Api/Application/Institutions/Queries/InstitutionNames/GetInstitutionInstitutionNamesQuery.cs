using MediatR;

namespace ZUEPC.Application.Institutions.Queries.InstitutionNames;

public class GetInstitutionInstitutionNamesQuery : IRequest<GetInstitutionInstitutionNamesQueryResponse>
{
	public long InstitutionId { get; set; }
}
