using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Base.Commands;

public class EPCUpdateCommandBase : UpdateCommandBase<long>
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime? VersionDate { get; set; }
}
