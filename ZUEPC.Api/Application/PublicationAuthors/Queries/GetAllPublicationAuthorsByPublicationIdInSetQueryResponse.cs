using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.PublicationAuthors;

namespace ZUEPC.Api.Application.PublicationAuthors.Queries;

public class GetAllPublicationAuthorsByPublicationIdInSetQueryResponse : ResponseWithDataBase<IEnumerable<PublicationAuthor>>
{
}