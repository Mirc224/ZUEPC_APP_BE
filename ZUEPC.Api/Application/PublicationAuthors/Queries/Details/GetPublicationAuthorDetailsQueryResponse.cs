using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.PublicationAuthors.Queries.Details;

public class GetPublicationAuthorDetailsQueryResponse : ResponseWithDataBase<IEnumerable<PublicationAuthorDetails>>
{
}