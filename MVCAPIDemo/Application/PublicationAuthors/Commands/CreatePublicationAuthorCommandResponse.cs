using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class CreatePublicationAuthorCommandResponse : ResponseBase
{
	public PublicationAuthor PublicationAuthor { get; set; }
}