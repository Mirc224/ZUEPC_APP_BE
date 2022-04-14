using ZUEPC.EvidencePublication.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Entities.Previews;

public class InstitutionPreview
{
	public long Id { get; set; }
	public int? Level { get; set; }
	public string? InstitutionType { get; set; }
	public IEnumerable<InstitutionName>? Names { get; set; }
	public IEnumerable<InstitutionExternDatabaseId>? ExternDatabaseIds { get; set; }
}
