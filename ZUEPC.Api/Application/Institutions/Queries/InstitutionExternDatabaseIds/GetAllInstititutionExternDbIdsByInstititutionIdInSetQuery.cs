using MediatR;

namespace ZUEPC.Api.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetAllInstititutionExternDbIdsByInstititutionIdInSetQuery : IRequest<GetAllInstititutionExternDbIdsByInstititutionIdInSetQueryResponse>
{
	public IEnumerable<long> InstitutionIds { get; set; }
}
