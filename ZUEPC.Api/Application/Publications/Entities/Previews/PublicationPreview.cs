using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Application.Publications.Entities.Previews;

public class PublicationPreview
{
	public long Id { get; set; }
	public int? PublishYear { get; set; }
	public string? DocumentType { get; set; }
	public ICollection<PublicationName>? Names { get; set; }
	public ICollection<PublicationIdentifier>? Identifiers { get; set; }
	public ICollection<PublicationExternDatabaseId>? ExternDatabaseIds { get; set; }
	public ICollection<PublicationAuthorDetails>? Authors { get; set; }
	public ICollection<PublicationActivity>? PublicationActivities { get; set; }
}
