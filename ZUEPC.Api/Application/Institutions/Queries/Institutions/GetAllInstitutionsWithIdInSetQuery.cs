using MediatR;

namespace ZUEPC.Api.Application.Institutions.Queries.Institutions;

public class GetAllInstitutionsWithIdInSetQuery: IRequest<GetAllInstitutionsWithIdInSetQueryResponse>
{
	public IEnumerable<long> InstitutionIds { get; set; }
}
