using MediatR;
using ZUEPC.Application.Persons.Entities.Inputs.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Entities.Inputs.PersonNames;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class CreatePersonWithDetailsCommand : EPCCreateBaseCommand, IRequest<CreatePersonWithDetailsCommandResponse>
{
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }
	public IEnumerable<PersonNameCreateDto>? Names { get; set; }
	public IEnumerable<PersonExternDatabaseIdCreateDto>? ExternDatabaseIds { get; set; }
}
