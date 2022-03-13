﻿using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class DeletePublicationExternDatabaseIdCommand : DeleteModelCommandBase, IRequest<DeletePublicationExternDatabaseIdCommandResponse>
{
}