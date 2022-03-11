using ZUEPC.Application.Institutions.Entities.Inputs.Common;
using ZUEPC.Common.Entities.Inputs;

namespace ZUEPC.Application.Institutions.Entities.Inputs.InstitutionExternDatabaseIds;

public class InstitutionExternDatabaseIdBaseDto : InstitutionPropertyBaseDto
{
	public string ExternIdentifierValue { get; set; }
}
