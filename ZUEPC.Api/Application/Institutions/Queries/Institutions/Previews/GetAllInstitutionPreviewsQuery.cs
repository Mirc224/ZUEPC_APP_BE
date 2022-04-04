using MediatR;
using ZUEPC.Api.Common.CQRS.Queries;
using ZUEPC.Common.CQRS.Queries;
using ZUEPC.Base.QueryFilters;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetAllInstitutionPreviewsQuery :
	PaginationWithFilterQueryBase<InstitutionFilter>,
	IRequest<GetAllInstitutionPreviewsQueryResponse>
{
}
