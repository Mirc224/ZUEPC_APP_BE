using ZUEPC.Base.Enums.Common;

namespace ZUEPC.DataAccess.Models.Common;

public abstract class EPCBaseModel
{
	public long Id { get; set; }
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime VersionDate { get; set; }
}
