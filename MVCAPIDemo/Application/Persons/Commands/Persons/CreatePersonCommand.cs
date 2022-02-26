using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class CreatePersonCommand : EPCCreateBaseCommand, IRequest<CreatePersonCommandResponse>
{
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }
}
