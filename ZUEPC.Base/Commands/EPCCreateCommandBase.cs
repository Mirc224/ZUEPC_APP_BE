using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Base.Commands;

public abstract class EPCCreateCommandBase
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime? VersionDate { get; set; }
}
