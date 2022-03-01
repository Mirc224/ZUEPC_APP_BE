using ZUEPC.Base.Enums.Common;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;

namespace ZUEPC.Application.Publications.Entities.Details;

public class PublicationDetails
{
	public long Id { get; set; }
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }
	public ICollection<PublicationName> Names { get; set; }
	public ICollection<PublicationIdentifier> Identifiers { get; set; }
	public ICollection<PublicationExternDatabaseId> ExternDatabaseIds { get; set; }
	public ICollection<PublicationAuthorDetails> PublicationAuthors { get; set; }
	public ICollection<Publication> RelatedPublications { get; set; }
	public ICollection<PublicationActivity> PublicationActivities { get; set; }
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime VersionDate { get; set; }
}
