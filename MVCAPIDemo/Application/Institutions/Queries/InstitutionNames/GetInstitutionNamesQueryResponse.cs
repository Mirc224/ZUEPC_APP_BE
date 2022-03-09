using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Queries.InstitutionNames;

public class GetInstitutionNamesQueryResponse : ResponseBaseWithData<ICollection<InstitutionName>>
{
}