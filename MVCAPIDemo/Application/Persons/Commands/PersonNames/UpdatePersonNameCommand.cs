using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class UpdatePersonNameCommand : EPCUpdateCommandBase, IRequest<UpdatePersonNameCommandResponse>
{
	public long PersonId { get; set; }
	public string? NameType { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
}
