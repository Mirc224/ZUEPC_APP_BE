using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.Publication;

public class PublicationModel : EPCModelBase
{
	public int? PublishYear { get; set; }
	public string? DocumentType { get; set; }
}
