using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetPublicationExternDatabaseIdQueryResponse : ResponseBase
{
	public PublicationExternDatabaseId? PublicationExternDatabaseId { get; set; }
}