using ZUEPC.Base.Enums.Common;

namespace ZUEPC.DataAccess.Models.Common;

public abstract class EPCBaseModel : ModelBase
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime VersionDate { get; set; }
}
