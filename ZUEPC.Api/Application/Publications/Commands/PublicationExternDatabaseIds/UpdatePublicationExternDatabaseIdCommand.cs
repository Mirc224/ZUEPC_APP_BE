﻿using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class UpdatePublicationExternDatabaseIdCommand : 
	EPCUpdateCommandBase,
	IRequest<UpdatePublicationExternDatabaseIdCommandResponse>
{
	public long PublicationId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}
