using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.PublicationAuthors.Queries.Details;

public class GetPublicationAuthorDetailsQueryResponse : ResponseWithDataBase<ICollection<PublicationAuthorDetails>>
{
}