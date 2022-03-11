using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Common.Entities;

public class DetailsBase
{
	public long Id { get; set; }
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime VersionDate { get; set; }
}
