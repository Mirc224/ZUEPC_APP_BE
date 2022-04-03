using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class DeletePersonCommand : 
	DeleteModelCommandBase<long>, 
	IRequest<DeletePersonCommandResponse>
{
}
