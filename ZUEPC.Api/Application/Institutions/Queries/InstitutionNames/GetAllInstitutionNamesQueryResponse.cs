using ZUEPC.EvidencePublication.Base.Domain.Institutions;
using ZUEPC.Responses;

namespace ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;

public class GetAllInstitutionNamesQueryResponse :
	PagedResponseBase<IEnumerable<InstitutionName>>
{
}