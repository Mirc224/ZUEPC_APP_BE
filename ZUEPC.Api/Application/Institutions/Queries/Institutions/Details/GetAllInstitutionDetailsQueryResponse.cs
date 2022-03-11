using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Responses;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetAllInstitutionDetailsQueryResponse
	: PagedResponseBase<IEnumerable<InstitutionDetails>>
{
}
