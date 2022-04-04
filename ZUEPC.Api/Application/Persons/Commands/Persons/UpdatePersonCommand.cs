using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class UpdatePersonCommand : EPCUpdateCommandBase, IRequest<UpdatePersonCommandResponse>
{
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }
}
