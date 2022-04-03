using ZUEPC.Base.Enums.Common;

namespace ZUEPC.DataAccess.Interfaces;

public interface IEPCItemBase
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime VersionDate { get; set; }
}
