using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Publication;

public class PublicationExternDatabaseIdModel : EPCExternDatabaseIdBaseModel
{
	[ExcludeFromUpdate]
	public long PublicationId { get; set; }
}
