using MediatR;

namespace ZUEPC.Application.Institutions.Queries.InstitutionNames;

public class GetInstitutionNamesQuery : IRequest<GetInstitutionNamesQueryResponse>
{
	public long InstitutionId { get; set; }
}
