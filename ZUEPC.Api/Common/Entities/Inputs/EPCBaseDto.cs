using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Common.Entities.Inputs;

public abstract class EPCBaseDto
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime? VersionDate { get; set; }
}
