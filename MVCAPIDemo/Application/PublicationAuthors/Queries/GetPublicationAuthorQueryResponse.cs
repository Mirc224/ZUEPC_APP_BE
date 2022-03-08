using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Queries;

public class GetPublicationAuthorQueryResponse : ResponseBase
{
	public PublicationAuthor? PublicationAuthor { get; set; }
}