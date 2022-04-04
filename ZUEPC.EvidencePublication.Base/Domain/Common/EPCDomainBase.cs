using ZUEPC.Base.Enums.Common;
using ZUEPC.Base.ItemInterfaces;

namespace ZUEPC.EvidencePublication.Domain.Common;

public class EPCDomainBase : 
	DomainBase,
	IEPCItemBase,
	IItemWithID<long>
{
	public long Id { get; set; }
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime VersionDate { get; set; }
}
