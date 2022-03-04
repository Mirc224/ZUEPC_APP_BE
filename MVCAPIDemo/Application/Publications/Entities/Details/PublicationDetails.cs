using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Application.RelatedPublications.Entities.Details;
using ZUEPC.Common.Entities;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Entities.Details;

public class PublicationDetails : DetailsBase
{
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }
	public ICollection<PublicationName>? Names { get; set; }
	public ICollection<PublicationIdentifier>? Identifiers { get; set; }
	public ICollection<PublicationExternDatabaseId>? ExternDatabaseIds { get; set; }
	public ICollection<PublicationAuthorDetails>? PublicationAuthors { get; set; }
	public ICollection<RelatedPublicationDetails>? RelatedPublications { get; set; }
	public ICollection<PublicationActivity>? PublicationActivities { get; set; }
}
