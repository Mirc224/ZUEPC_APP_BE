﻿using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class CreatePublicationAuthorCommand : EPCCreateBaseCommand, IRequest<CreatePublicationAuthorCommandResponse>
{
	public long PublicationId { get; set; }
	public long PersonId { get; set; }
	public long InstitutionId { get; set; }
	public double? ContributionRatio { get; set; }
	public string? PersonRole { get; set; }
}
