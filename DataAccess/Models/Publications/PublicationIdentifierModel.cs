using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Publication;

public class PublicationIdentifierModel : EPCModelBase
{
	public long PublicationId { get; set; }
	public string? IdentifierValue { get; set; }
	public string? IdentifierName { get; set; }
	public string? ISForm { get; set; }
}
