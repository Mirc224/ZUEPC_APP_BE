using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class DeletePublicationIdentifierCommand : EPCDeleteCommandBase, IRequest<DeletePublicationIdentifierCommandResponse>
{
}
