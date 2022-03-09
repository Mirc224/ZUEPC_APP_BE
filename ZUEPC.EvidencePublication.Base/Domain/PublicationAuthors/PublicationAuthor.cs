using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Common.Interfaces;

namespace ZUEPC.EvidencePublication.Base.PublicationAuthors;

public class PublicationAuthor : 
	EPCBase,
	IPublicationRelated
{
	public long PublicationId { get; set; }
	public long PersonId { get; set; }
	public long InstitutionId { get; set; }
	public double? ContributionRatio { get; set; }
	public string? Role { get; set; }
}
