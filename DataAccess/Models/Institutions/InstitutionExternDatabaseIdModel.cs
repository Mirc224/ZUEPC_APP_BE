using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Institution;

public class InstitutionExternDatabaseIdModel : EPCExternDatabaseIdBaseModel
{
	[ExcludeFromUpdate]
	public long InstitutionId { get; set; }
}
