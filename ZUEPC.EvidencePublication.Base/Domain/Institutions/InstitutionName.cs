using ZUEPC.EvidencePublication.Base.Domain.Common;
using ZUEPC.EvidencePublication.Base.Domain.Common.Interfaces;

namespace ZUEPC.EvidencePublication.Base.Domain.Institutions;

public class InstitutionName : EPCBase, IInstitutionRelated
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public long InstitutionId { get; set; }
	public string Name { get; set; }
	public string? NameType { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
