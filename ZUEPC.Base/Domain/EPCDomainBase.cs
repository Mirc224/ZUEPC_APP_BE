using ZUEPC.Base.Enums.Common;
using ZUEPC.Base.ItemInterfaces;

namespace ZUEPC.Base.Domain;

public abstract class EPCDomainBase : 
	DomainBase,
	IEPCItemBase,
	IItemWithID<long>
{
	public long Id { get; set; }
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime VersionDate { get; set; }
}
