using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Base.Entities;

public class ItemDetailsBase
{
	public long Id { get; set; }
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime VersionDate { get; set; }
}
