using ZUEPC.EvidencePublication.Domain.Institutions;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetAllInstitutionsQueryResponse :
	PaginatedResponseBase<IEnumerable<Institution>>
{
}