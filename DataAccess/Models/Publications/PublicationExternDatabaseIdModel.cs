using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Publication;

public class PublicationExternDatabaseIdModel :
	EPCModelBase,
	IEPCItemWithExternIdentifier
{
	[ExcludeFromUpdate]
	public long PublicationId { get; set; }
	public string? ExternIdentifierValue { get; set; }
}
