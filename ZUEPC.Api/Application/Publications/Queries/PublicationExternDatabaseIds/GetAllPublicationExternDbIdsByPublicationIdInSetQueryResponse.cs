using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.Publications;

namespace ZUEPC.Api.Application.Publications.Queries.PublicationExternDatabaseIds;

public class GetAllPublicationExternDbIdsByPublicationIdInSetQueryResponse : 
	ResponseWithDataBase<IEnumerable<PublicationExternDatabaseId>>
{
}