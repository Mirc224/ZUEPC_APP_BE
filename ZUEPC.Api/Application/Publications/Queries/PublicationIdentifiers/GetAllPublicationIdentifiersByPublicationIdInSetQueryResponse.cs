using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationIdentifiers;

public class GetAllPublicationIdentifiersByPublicationIdInSetQueryResponse : ResponseWithDataBase<IEnumerable<PublicationIdentifier>>
{
}