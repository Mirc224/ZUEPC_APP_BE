using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Details;

public class GetAllInstitutionDetailsQuery :
	PaginatedQueryWithFilterBase<InstitutionFilter>,
	IRequest<GetAllInstitutionDetailsQueryResponse>
{
}
