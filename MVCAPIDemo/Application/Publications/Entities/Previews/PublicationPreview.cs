using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Entities.Previews;

public class PublicationPreview
{
	public long Id { get; set; }
	public ICollection<PublicationName>? PublicationNames { get; set; }
	public ICollection<PublicationIdentifier>? PublicationIdentifiers { get; set; }
	public ICollection<PublicationAuthorDetails>? PublicationAuthors { get; set; }
}
