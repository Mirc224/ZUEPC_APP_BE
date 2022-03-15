using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.PublicationActivity;

public class PublicationActivityModel : EPCModelBase
{
	[ExcludeFromUpdate]
	public long PublicationId { get; set; }
	public int? ActivityYear { get; set; }
	public string? Category { get; set; }
	public string? GovernmentGrant { get; set; }
}
