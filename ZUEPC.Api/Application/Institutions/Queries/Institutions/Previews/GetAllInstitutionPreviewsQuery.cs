using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetAllInstitutionPreviewsQuery
	: PaginationQueryWithUriBase, IRequest<GetAllInstitutionPreviewsQueryResponse>
{
}
