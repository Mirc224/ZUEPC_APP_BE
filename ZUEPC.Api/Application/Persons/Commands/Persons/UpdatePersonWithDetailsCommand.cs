using MediatR;
using ZUEPC.Application.Persons.Entities.Inputs.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Entities.Inputs.PersonNames;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class UpdatePersonWithDetailsCommand : EPCUpdateCommandBase, IRequest<UpdatePersonWithDetailsCommandResponse>
{
	public int? BirthYear { get; set; }
	public int? DeathYear { get; set; }
	public IEnumerable<PersonNameCreateDto>? NamesToInsert { get; set; }
	public IEnumerable<PersonNameUpdateDto>? NamesToUpdate { get; set; }
	public IEnumerable<long>? NamesToDelete { get; set; }
	public IEnumerable<PersonExternDatabaseIdCreateDto>? ExternDatabaseIdsToInsert { get; set; }
	public IEnumerable<PersonExternDatabaseIdUpdateDto>? ExternDatabaseIdsToUpdate { get; set; }
	public IEnumerable<long>? ExternDatabaseIdsToDelete { get; set; }
}
