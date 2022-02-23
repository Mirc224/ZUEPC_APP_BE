using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Institution;

public class InstitutionExternDatabaseIdModel : EPCBaseModel
{
	public long InstitutionId { get; set; }
	public string? InstitutionExternDbId { get; set; }
}
