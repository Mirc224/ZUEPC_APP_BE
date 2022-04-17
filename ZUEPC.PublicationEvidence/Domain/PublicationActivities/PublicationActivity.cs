using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Base.Domain;

namespace ZUEPC.EvidencePublication.Domain.PublicationActivities;

public class PublicationActivity : 
	EPCDomainBase, 
	IPublicationRelated
{
	public long PublicationId { get; set; }
	public int? ActivityYear { get; set; }
	public string? Category { get; set; }
	public string? GovernmentGrant { get; set; }
}
