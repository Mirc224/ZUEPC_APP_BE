using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Institution;

public class InstitutionNameModel : EPCModelBase
{
	public long InstitutionId { get; set; }
	public string? NameType { get; set; }
	public string? Name { get; set; }
}
