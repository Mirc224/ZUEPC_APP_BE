using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class DeletePublicationIdentifierCommand : EPCDeleteBaseCommand, IRequest<DeletePublicationIdentifierCommandResponse>
{
}
