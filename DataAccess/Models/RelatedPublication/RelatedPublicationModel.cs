using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.RelatedPublication;

public class RelatedPublicationModel : EPCBaseModel
{
	public long PublicationId { get; set; }
	public long RelatedPublicationId { get; set; }
	public string? RelationType { get; set; }
	public string? CitationCategory { get; set; }
}
