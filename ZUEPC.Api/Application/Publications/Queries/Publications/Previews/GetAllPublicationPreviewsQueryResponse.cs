using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Publications.Queries.Publications.Previews;

public class GetAllPublicationPreviewsQueryResponse : PaginatedResponseBase<IEnumerable<PublicationPreview>>
{
}