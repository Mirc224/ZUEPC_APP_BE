using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetPublicationRelatedPublicationsQueryResponse : ResponseBase
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public IEnumerable<RelatedPublication> RelatedPublications { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}