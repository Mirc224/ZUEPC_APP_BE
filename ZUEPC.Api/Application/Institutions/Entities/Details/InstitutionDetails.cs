using ZUEPC.Base.Entities;
using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Entities.Details;

public class InstitutionDetails : ItemDetailsBase
{
	public int? Level { get; set; }
	public string? InstitutionType { get; set; }
	public ICollection<InstitutionName>? Names { get; set; }
	public ICollection<InstitutionExternDatabaseId>? ExternDatabaseIds { get; set; }
}
