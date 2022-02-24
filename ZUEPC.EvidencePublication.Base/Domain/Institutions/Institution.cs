using ZUEPC.EvidencePublication.Base.Domain.Common;

namespace ZUEPC.EvidencePublication.Base.Domain.Institutions;

public class Institution : EPCBase
{
		public int? Level { get; set; }
	public string? InstitutionType { get; set; }
}
