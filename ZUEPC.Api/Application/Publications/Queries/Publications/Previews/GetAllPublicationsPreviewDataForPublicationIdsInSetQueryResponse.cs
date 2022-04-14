using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Domain.Publications;
using ZUEPC.EvidencePublication.PublicationAuthors;

namespace ZUEPC.Api.Application.Publications.Queries.Publications.Previews;

public class GetAllPublicationsPreviewDataForPublicationIdsInSetQueryResponse : ResponseBase
{
	public IEnumerable<PublicationName> PublicationNames { get; set; }
	public IEnumerable<PublicationIdentifier> PublicationIdentifiers { get; set; }
	public IEnumerable<PublicationExternDatabaseId> PublicationExternDbIds { get; set; }
	public IEnumerable<PublicationActivity> PublicationActivities { get; set; }
	public IEnumerable<PublicationAuthor> PublicationAuthors { get; set; }
}