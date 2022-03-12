using ZUEPC.Base.Commands;
using ZUEPC.Base.Enums.Common;

namespace ZUEPC.EvidencePublication.Base.Commands;

public class EPCUpdateCommandBase : UpdateCommandBase
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime? VersionDate { get; set; }
}
