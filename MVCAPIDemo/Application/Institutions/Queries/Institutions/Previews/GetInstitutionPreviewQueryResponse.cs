using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Common.Responses;

namespace ZUEPC.Application.Institutions.Queries.Institutions.Previews;

public class GetInstitutionPreviewQueryResponse : ResponseBase
{
	public InstitutionPreview? InstitutionPreview { get; set; }
}