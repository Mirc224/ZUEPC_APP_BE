using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Base.Responses;

namespace ZUEPC.Api.Application.PublicationAuthors.Queries.Details;

public class GetAllPublicationAuthorsDetailsByPublicationIdInSetQueryResponse : ResponseWithDataBase<IEnumerable<PublicationAuthorDetails>>
{
}