using ZUEPC.Common.Entities.Inputs;

namespace ZUEPC.Application.Institutions.Entities.Inputs.InstitutionNames;

public class InstitutionNameBaseDto : EPCBaseDto
{
	public long? InstitutionId { get; set; }
	public string? NameType { get; set; }
	public string? Name { get; set; }
}
