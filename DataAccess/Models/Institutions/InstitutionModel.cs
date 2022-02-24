using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Institution;

public class InstitutionModel: EPCBaseModel
{
	public int? Level { get; set; }
	public string? InstitutionType { get; set; }
}
