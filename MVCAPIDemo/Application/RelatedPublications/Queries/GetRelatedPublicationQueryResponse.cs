using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetRelatedPublicationQueryResponse : ResponseBase
{
	public RelatedPublication? RelatedPublication { get; set; }
}