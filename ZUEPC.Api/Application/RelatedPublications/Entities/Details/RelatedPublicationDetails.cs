using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Common.Entities;

namespace ZUEPC.Application.RelatedPublications.Entities.Details;

public class RelatedPublicationDetails : DetailsBase
{
	public PublicationPreview? RelatedPublication { get; set; }
	public string? RelationType { get; set; }
	public string? CitationCategory { get; set; }
}
