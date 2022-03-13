using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class DeletePublicationCommand : DeleteModelCommandBase, IRequest<DeletePublicationCommandResponse>
{
}
