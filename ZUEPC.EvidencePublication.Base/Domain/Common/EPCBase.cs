using ZUEPC.Base.Enums.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Common;

public class EPCBase
{
	public long Id { get; set; }
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime VersionDate { get; set; }
}
