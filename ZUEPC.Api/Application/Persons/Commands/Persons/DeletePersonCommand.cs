using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class DeletePersonCommand : 
	EPCDeleteCommandBase<long>, 
	IRequest<DeletePersonCommandResponse>
{
}
