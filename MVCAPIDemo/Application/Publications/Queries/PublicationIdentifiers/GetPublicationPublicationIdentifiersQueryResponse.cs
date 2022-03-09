using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetPublicationPublicationIdentifiersQueryResponse : ResponseBaseWithData<ICollection<PublicationIdentifier>>
{
}