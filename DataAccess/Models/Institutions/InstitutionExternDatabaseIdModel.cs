using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.DataAccess.Interfaces;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Institution;

public class InstitutionExternDatabaseIdModel : 
	EPCModelBase,
	IEPCItemWithExternIdentifier
{
	[ExcludeFromUpdate]
	public long InstitutionId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}
