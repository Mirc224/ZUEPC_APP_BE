using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationNames;

public class UpdatePublicationNameCommand : EPCUpdateCommandBase, IRequest<UpdatePublicationNameCommandResponse>
{
	public long PublicationId { get; set; }
	public string? Name { get; set; }
	public string? NameType { get; set; }
}
