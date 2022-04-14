using ZUEPC.Base.Responses;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Api.Application.Institutions.Queries.Institutions.Previews.BaseHandlers;

public class GetAllInstitutionPreviewDataForInstitutionIdsInSetQueryResponse : ResponseBase
{
	public IEnumerable<InstitutionName> InstitutionNames { get; set; }
	public IEnumerable<InstitutionExternDatabaseId> InstitutionExternDatabaseIds { get; set; }
}