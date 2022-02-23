using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Publication;

public class PublicationExternDatabaseIdModel : EPCBaseModel
{
	public long PublicationId { get; set; }
	public string? PublicationExternDatabaseId { get; set; }
}
