using MediatR;

namespace ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;

public class GetAllInstititutionNamesByInstititutionIdInSetQuery : IRequest<GetAllInstititutionNamesByInstititutionIdInSetQueryResponse>
{
	public IEnumerable<long> InstitutionIds { get; set; }
}
