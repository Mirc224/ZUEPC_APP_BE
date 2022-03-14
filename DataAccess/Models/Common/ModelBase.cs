using ZUEPC.DataAccess.Attributes.ModelAttributes;

namespace ZUEPC.DataAccess.Models.Common;

public abstract class ModelBase
{
	[ExcludeFromUpdate]
	[ExcludeFromInsert]
	public long Id { get; set; }
	[ExcludeFromUpdate]
	public DateTime CreatedAt { get; set; }
}
