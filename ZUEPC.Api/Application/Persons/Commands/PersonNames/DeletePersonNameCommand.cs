using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class DeletePersonNameCommand : 
	DeleteModelCommandBase<long>,
	IRequest<DeletePersonNameCommandResponse>
{
}
