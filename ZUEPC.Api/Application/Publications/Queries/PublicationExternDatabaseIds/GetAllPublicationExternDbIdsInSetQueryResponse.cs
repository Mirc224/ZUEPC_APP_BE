using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetAllPublicationExternDbIdsInSetQueryResponse : ResponseWithDataBase<ICollection<PublicationExternDatabaseId>>
{
}