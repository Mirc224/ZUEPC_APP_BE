using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Publication;

public class PublicationModel : EPCModelBase
{
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }
}
