using MediatR;

namespace ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetInstitutionInstitutionExternDatabaseIdsQuery : IRequest<GetInstitutionInstitutionExternDatabaseIdsQueryResponse>
{
	public long InstitutionId { get; set; }
}
