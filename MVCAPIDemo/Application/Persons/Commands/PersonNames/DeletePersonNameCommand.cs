using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class DeletePersonNameCommand : EPCDeleteBaseCommand, IRequest<DeletePersonNameCommandResponse>
{
}
