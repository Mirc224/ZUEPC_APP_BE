using ZUEPC.Base.Enums.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Common;

public class EPCDomainBase : DomainBase
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime VersionDate { get; set; }
}
