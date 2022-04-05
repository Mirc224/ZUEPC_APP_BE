using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Persons.Queries.Persons.Previews;

public class GetAllPersonPreviewsQueryResponse : PaginatedResponseBase<IEnumerable<PersonPreview>>
{
}