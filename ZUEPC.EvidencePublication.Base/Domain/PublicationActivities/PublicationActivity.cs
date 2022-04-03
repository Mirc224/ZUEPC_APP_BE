using ZUEPC.DataAccess.Interfaces;
using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;

public class PublicationActivity : 
	EPCDomainBase, 
	IPublicationRelated
{
	public long PublicationId { get; set; }
	public int? ActivityYear { get; set; }
	public string? Category { get; set; }
	public string? GovernmentGrant { get; set; }
}
