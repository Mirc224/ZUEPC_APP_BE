using ZUEPC.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetAllPublicationIdentifiersInSetQueryResponse : ResponseWithDataBase<ICollection<PublicationIdentifier>>
{
}
