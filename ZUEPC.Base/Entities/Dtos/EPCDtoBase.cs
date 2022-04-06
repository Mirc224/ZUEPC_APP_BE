using ZUEPC.Base.Enums.Common;

namespace ZUEPC.Base.Entities.Dtos;

public abstract class EPCDtoBase
{
	public OriginSourceType OriginSourceType { get; set; }
	public DateTime? VersionDate { get; set; }
}
