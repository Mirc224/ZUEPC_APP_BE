using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Responses;

namespace ZUEPC.Application.PublicationAuthors.Queries.Details;

public class GetPublicationAuthorDetailsQueryResponse : ResponseWithDataBase<ICollection<PublicationAuthorDetails>>
{
}