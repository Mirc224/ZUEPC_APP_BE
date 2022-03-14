using MediatR;
using ZUEPC.Common.CQRS.Queries;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetAllInstitutionPreviewsQuery
	: PaginationWithUriQueryBase, IRequest<GetAllInstitutionPreviewsQueryResponse>
{
}
