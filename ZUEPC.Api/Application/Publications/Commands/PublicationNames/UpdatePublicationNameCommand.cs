﻿using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.PublicationNames;

public class UpdatePublicationNameCommand : EPCUpdateCommandBase, IRequest<UpdatePublicationNameCommandResponse>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public long PublicationId { get; set; }
	public string Name { get; set; }
	public string? NameType { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
