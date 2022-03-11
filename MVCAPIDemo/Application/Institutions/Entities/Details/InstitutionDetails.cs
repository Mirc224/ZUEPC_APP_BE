using ZUEPC.Common.Entities;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Entities.Details;

public class InstitutionDetails : DetailsBase
{
	public int? Level { get; set; }
	public string? InstitutionType { get; set; }
	public ICollection<InstitutionName>? Names { get; set; }
	public ICollection<InstitutionExternDatabaseId>? ExternDatabaseIds { get; set; }
}
