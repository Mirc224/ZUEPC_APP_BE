using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.PublicationAuthors.Queries.Previews;

public class GetPublicationAuthorsPreviewsQueryResponse : ResponseWithDataBase<ICollection<PublicationAuthorDetails>>
{
}