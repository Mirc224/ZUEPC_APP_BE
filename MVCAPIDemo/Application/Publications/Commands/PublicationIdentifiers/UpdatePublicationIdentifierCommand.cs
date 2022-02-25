using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class UpdatePublicationIdentifierCommand : EPCUpdateBaseCommand, IRequest<UpdatePublicationIdentifierCommandResponse>
{
	public string? IdentifierValue { get; set; }
	public string? IdentifierName { get; set; }
	public string? ISForm { get; set; }
}
