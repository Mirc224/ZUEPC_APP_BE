using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Common.Entities.Inputs;

public class EPCBaseDto
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime? VersionDate { get; set; }
}
