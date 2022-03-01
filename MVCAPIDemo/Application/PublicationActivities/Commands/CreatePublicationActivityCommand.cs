﻿using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class CreatePublicationActivityCommand : EPCCreateBaseCommand, IRequest<CreatePublicationActivityCommandResponse>
{
	public string? Category { get; set; }
	public string? GovernmentGrant { get; set; }
	public int? ActivityYear { get; set; }
}
