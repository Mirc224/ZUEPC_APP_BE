using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationNames;

public class GetAllPublicationNamesByPublicationIdInSetQueryResponse : ResponseWithDataBase<IEnumerable<PublicationName>>
{
}