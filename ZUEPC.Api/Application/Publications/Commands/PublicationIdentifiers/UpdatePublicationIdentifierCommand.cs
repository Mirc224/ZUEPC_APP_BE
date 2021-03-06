using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class UpdatePublicationIdentifierCommand : EPCUpdateCommandBase, IRequest<UpdatePublicationIdentifierCommandResponse>
{
	public long PublicationId { get; set; }
	public string? IdentifierValue { get; set; }
	public string? IdentifierName { get; set; }
	public string? ISForm { get; set; }
}
