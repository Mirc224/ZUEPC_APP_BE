using ZUEPC.DataAccess.Attributes.ModelAttributes;
using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Publication;

public class PublicationNameModel : EPCModelBase
{
	[ExcludeFromUpdate]
	public long PublicationId { get; set; }
	public string? Name { get; set; }
	public string? NameType { get; set; }
}
