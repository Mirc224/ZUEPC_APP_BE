using MediatR;
using ZUEPC.Common.CQRS.Queries;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetAllPersonPreviewsQuery : PaginationWithUriQueryBase, IRequest<GetAllPersonPreviewsQueryResponse>
{
}
