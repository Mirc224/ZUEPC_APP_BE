using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;

public class GetInstitutionExternDatabaseIdsQueryResponse : ResponseBaseWithData<ICollection<InstitutionExternDatabaseId>>
{
}