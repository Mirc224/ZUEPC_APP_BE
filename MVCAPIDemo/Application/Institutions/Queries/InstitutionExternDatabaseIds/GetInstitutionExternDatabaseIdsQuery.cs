using MediatR;

namespace ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetInstitutionExternDatabaseIdsQuery : IRequest<GetInstitutionExternDatabaseIdsQueryResponse>
{
	public long InstitutionId { get; set; }
}
