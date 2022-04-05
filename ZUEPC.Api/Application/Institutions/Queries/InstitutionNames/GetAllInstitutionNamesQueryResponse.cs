using ZUEPC.EvidencePublication.Domain.Institutions;
using ZUEPC.Base.Responses;

namespace ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;

public class GetAllInstitutionNamesQueryResponse :
	PaginatedResponseBase<IEnumerable<InstitutionName>>
{
}