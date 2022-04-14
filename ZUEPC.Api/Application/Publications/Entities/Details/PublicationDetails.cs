using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Application.RelatedPublications.Entities.Details;
using ZUEPC.Base.Entities;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Application.Publications.Entities.Details;

public class PublicationDetails : ItemDetailsBase
{
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }
	public IEnumerable<PublicationName>? Names { get; set; }
	public IEnumerable<PublicationIdentifier>? Identifiers { get; set; }
	public IEnumerable<PublicationExternDatabaseId>? ExternDatabaseIds { get; set; }
	public IEnumerable<PublicationAuthorDetails>? Authors { get; set; }
	public IEnumerable<RelatedPublicationDetails>? RelatedPublications { get; set; }
	public IEnumerable<PublicationActivity>? PublicationActivities { get; set; }
}
