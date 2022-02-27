using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class CreatePublicationIdentifierCommandResponse : ResponseBase
{
	public PublicationIdentifier CreatedPublicationIdentifier { get; set; }
}