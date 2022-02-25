using ZUEPC.Base.Enums.Common;

namespace ZUEPC.EvidencePublication.Base.Commands;

public class EPCUpdateBaseCommand
{
	public long Id { get; set; }
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime? VersionDate { get; set; }
}
