using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class DeletePublicationActivityCommand : EPCDeleteCommandBase, IRequest<DeletePublicationActivityCommandResponse>
{
}
