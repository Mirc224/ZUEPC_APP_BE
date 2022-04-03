﻿using ZUEPC.DataAccess.Interfaces;
using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.PublicationAuthors;

public class PublicationAuthor : 
	EPCDomainBase,
	IPublicationRelated
{
	public long PublicationId { get; set; }
	public long PersonId { get; set; }
	public long InstitutionId { get; set; }
	public double? ContributionRatio { get; set; }
	public string? Role { get; set; }
}
