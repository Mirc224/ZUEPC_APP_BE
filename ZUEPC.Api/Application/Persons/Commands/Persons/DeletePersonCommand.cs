using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class DeletePersonCommand : 
	DeleteModelCommandBase<long>, 
	IRequest<DeletePersonCommandResponse>
{
}
