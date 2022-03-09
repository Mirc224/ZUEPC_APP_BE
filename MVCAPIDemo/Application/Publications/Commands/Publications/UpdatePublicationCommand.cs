﻿using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class UpdatePublicationCommand : EPCUpdateCommandBase, IRequest<UpdatePublicationCommandResponse>
{
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }
}
