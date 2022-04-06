using MediatR;
using ZUEPC.Base.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetAllInstitutionPreviewsQuery :
	PaginatedQueryWithFilterBase<InstitutionFilter>,
	IRequest<GetAllInstitutionPreviewsQueryResponse>
{
}
