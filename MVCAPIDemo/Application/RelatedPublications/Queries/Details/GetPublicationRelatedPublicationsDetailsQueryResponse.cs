using ZUEPC.Application.RelatedPublications.Entities.Details;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.RelatedPublications.Queries.Details;

public class GetPublicationRelatedPublicationsDetailsQueryResponse : ResponseBase
{
	public ICollection<RelatedPublicationDetails>? RelatedPublications { get; set; }
}