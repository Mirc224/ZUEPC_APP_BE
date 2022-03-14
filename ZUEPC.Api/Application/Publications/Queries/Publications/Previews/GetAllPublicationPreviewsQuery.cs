using MediatR;
using ZUEPC.Common.CQRS.Queries;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetAllPublicationPreviewsQuery : PaginationWithUriQueryBase, IRequest<GetAllPublicationPreviewsQueryResponse>
{
}
