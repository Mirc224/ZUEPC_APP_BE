using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.PublicationActivity;

public class PublicationActivityModel : EPCBaseModel
{
	public long PublicationId { get; set; }
	public string? Category { get; set; }
	public string? GovernmentGrant { get; set; }
	public int? ActivityYear { get; set; }
}
