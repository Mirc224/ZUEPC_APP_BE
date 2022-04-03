using ZUEPC.Base.Enums.Common;
using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.DataAccess.Interfaces;

namespace ZUEPC.DataAccess.Models.Common;

public abstract class EPCModelBase : 
	ModelBase, 
	IEPCItemBase,
	IItemWithID<long>
{
	[ExcludeFromUpdate]
	[ExcludeFromInsert]
	public long Id { get; set; }
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime VersionDate { get; set; }
}
