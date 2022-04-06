using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Import.Commands.Common;

public class ImportBaseResponse : ResponseBase
{
	public IEnumerable<long>? PublicationsIds { get; set; }
}
