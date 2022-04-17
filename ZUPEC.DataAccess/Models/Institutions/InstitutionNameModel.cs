using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Institution;

public class InstitutionNameModel : EPCModelBase
{
	[ExcludeFromUpdate]
	public long InstitutionId { get; set; }
	public string? NameType { get; set; }
	public string? Name { get; set; }
}
