using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Api.Application.Publications.Queries.Publications;

public class GetAllPublicationsWithIdInSetQueryResponse : ResponseWithDataBase<IEnumerable<Publication>>
{
}