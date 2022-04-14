using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Base.Entities;
using ZUEPC.Base.ItemInterfaces;

namespace ZUEPC.Application.RelatedPublications.Entities.Details;

public class RelatedPublicationDetails : 
	ItemDetailsBase,
	IPublicationRelated
{
	public PublicationPreview? RelatedPublication { get; set; }
	public string? RelationType { get; set; }
	public string? CitationCategory { get; set; }
	public long PublicationId { get; set; }
}
