using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationIdentifiers;

public class GetPublicationIdentifiersQueryResponse : ResponseBase
{
	public ICollection<PublicationIdentifier>? PublicationIdentifiers { get; set; }
}