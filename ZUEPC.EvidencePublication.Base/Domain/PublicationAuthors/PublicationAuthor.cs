using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Base.Domain;

namespace ZUEPC.EvidencePublication.PublicationAuthors;

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
