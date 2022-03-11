using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class DeletePersonNameCommand : EPCDeleteCommandBase, IRequest<DeletePersonNameCommandResponse>
{
}
