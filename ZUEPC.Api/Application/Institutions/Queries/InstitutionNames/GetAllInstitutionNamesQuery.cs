using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Api.Application.Institutions.Queries.InstitutionNames;

public class GetAllInstitutionNamesQuery:
	PaginatedQueryWithFilterBase<InstitutionNameFilter>,
	IRequest<GetAllInstitutionNamesQueryResponse>
{
}
