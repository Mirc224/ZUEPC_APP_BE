using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Publication;

public class PublicationIdentifierModel : EPCBaseModel
{
	public long PublicationId { get; set; }
	public string? PublicationIdentifierValue { get; set; }
	public string? IdentifierName { get; set; }
	public string? ISForm { get; set; }
}
