using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetAllInstitutionPreviewsQuery: EPCPaginationQueryWithUriBase, IRequest<GetAllInstitutionPreviewsQueryResponse>
{
}
