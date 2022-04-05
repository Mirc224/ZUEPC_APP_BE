using ZUEPC.Application.Institutions.Entities.Details;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetAllInstitutionDetailsQueryResponse
	: PaginatedResponseBase<IEnumerable<InstitutionDetails>>
{
}
