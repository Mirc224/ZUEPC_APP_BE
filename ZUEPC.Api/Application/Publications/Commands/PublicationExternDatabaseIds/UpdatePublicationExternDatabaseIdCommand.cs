using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class UpdatePublicationExternDatabaseIdCommand : 
	EPCUpdateCommandBase,
	IRequest<UpdatePublicationExternDatabaseIdCommandResponse>
{
	public long PublicationId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}
