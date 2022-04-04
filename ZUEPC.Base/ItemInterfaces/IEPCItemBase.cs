using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Base.ItemInterfaces;

public interface IEPCItemBase
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime VersionDate { get; set; }
}
