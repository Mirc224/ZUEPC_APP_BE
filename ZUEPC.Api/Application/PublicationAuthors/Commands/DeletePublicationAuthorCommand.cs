﻿using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class DeletePublicationAuthorCommand : DeleteModelCommandBase, IRequest<DeletePublicationAuthorCommandResponse>
{
}