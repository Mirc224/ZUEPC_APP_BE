using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Base.Commands;

public abstract class EPCUpdateCommandBase : UpdateCommandWithIdBase<long>
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime? VersionDate { get; set; }
}
