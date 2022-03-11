using ZUEPC.Responses;

namespace ZUEPC.Application.Import.Commands.Common;

public class ImportBaseResponse : ResponseBase
{
	public ICollection<long>? PublicationsIds { get; set; }
}
