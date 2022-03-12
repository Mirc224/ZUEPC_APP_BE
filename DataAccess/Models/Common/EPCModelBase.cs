using ZUEPC.Base.Enums.Common;

namespace ZUEPC.DataAccess.Models.Common;

public abstract class EPCModelBase : ModelBase
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime VersionDate { get; set; }
}
