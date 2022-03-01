using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Entities.Thumbnails;

public class PublicationThumbnail
{
	public long Id { get; set; }
	public ICollection<PublicationName> PublicationNames { get; set; }
	public ICollection<PublicationIdentifier> PublicationIdentifiers { get; set; }
}
