using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.RelatedPublications;

namespace ZUEPC.Api.Application.RelatedPublications.Queries;

public class GetAllRelatedPublicationByPublicationIdInSetQueryResponse : ResponseWithDataBase<IEnumerable<RelatedPublication>>
{
}