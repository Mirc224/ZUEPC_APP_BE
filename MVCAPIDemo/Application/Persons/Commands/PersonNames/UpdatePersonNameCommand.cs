using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class UpdatePersonNameCommand : EPCUpdateBaseCommand, IRequest<UpdatePersonNameCommandResponse>
{
	public string? NameType { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
}
