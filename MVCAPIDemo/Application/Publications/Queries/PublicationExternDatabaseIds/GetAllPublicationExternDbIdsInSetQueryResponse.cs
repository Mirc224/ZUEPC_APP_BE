using ZUEPC.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetAllPublicationExternDbIdsInSetQueryResponse : ResponseWithDataBase<ICollection<PublicationExternDatabaseId>>
{
}