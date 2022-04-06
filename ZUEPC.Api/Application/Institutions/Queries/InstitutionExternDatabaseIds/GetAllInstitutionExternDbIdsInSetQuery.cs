using MediatR;

namespace ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetAllInstitutionExternDbIdsInSetQuery : IRequest<GetAllInstitutionExternDbIdsInSetQueryResponse>
{
	public IEnumerable<string> SearchedExternIdentifiers { get; set; }
}
