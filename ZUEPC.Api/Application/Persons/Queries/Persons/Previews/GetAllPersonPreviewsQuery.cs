using MediatR;
using ZUEPC.Common.CQRS.Query;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetAllPersonPreviewsQuery : PaginationQueryWithUriBase, IRequest<GetAllPersonPreviewsQueryResponse>
{
}
