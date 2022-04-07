using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.RelatedPublications;

namespace ZUEPC.Application.RelatedPublications.Queries;

public class GetPublicationRelatedPublicationsQueryResponse : ResponseWithDataBase<IEnumerable<RelatedPublication>>
{
}