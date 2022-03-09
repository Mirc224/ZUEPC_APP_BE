using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationNames;

public class DeletePublicationNameCommand : EPCDeleteCommandBase, IRequest<DeletePublicationNameCommandResponse>
{
}
