using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;

public class PublicationActivity : EPCBase
{
	public long PublicationId { get; set; }
	public int? ActivityYear { get; set; }
	public string? Category { get; set; }
	public string? GovernmentGrant { get; set; }
}
