using ZUEPC.Application.Institutions.Entities.Inputs.Common;

namespace ZUEPC.Application.Institutions.Entities.Inputs.InstitutionNames;

public class InstitutionNameBaseDto : InstitutionPropertyBaseDto
{
	public string? NameType { get; set; }
	public string? Name { get; set; }
}
