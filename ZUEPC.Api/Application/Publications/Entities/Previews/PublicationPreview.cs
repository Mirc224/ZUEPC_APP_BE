using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Application.Publications.Entities.Previews;

public class PublicationPreview
{
	public long Id { get; set; }
	public int? PublishYear { get; set; }
	public string? DocumentType { get; set; }
	public IEnumerable<PublicationName>? Names { get; set; }
	public IEnumerable<PublicationIdentifier>? Identifiers { get; set; }
	public IEnumerable<PublicationExternDatabaseId>? ExternDatabaseIds { get; set; }
	public IEnumerable<PublicationAuthorDetails>? Authors { get; set; }
	public IEnumerable<PublicationActivity>? PublicationActivities { get; set; }
}
