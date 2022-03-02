using ZUEPC.EvidencePublication.Base.Domain.Institutions;

namespace ZUEPC.Application.Institutions.Entities.Previews;

public class InstitutionPreview
{
	public long Id { get; set; }
	public int? Level { get; set; }
	public string? InstitutionType { get; set; }
	public ICollection<InstitutionName>? InstitutionNames { get; set; }
}
