using ZUEPC.Base.Enums.Common;

namespace ZUEPC.EvidencePublication.Base.Commands;

public class EPCCreateCommandBase
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime? VersionDate { get; set; }
}
