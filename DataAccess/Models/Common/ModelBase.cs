using ZUEPC.DataAccess.Attributes.ModelAttributes;

namespace ZUEPC.DataAccess.Models.Common;

public abstract class ModelBase
{
	[ExcludeFromUpdate]
	public DateTime CreatedAt { get; set; }
}
