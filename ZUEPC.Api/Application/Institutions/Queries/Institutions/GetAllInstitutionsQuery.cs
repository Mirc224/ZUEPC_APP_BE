using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Institutions.Queries.Institutions;

public class GetAllInstitutionsQuery :
	PaginatedQueryWithFilterBase<InstitutionFilter>,
	IRequest<GetAllInstitutionsQueryResponse>
{
}
