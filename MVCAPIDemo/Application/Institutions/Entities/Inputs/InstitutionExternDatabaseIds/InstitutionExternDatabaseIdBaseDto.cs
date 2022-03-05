using ZUEPC.Common.Entities.Inputs;

namespace ZUEPC.Application.Institutions.Entities.Inputs.InstitutionExternDatabaseIds;

public class InstitutionExternDatabaseIdBaseDto : EPCBaseDto
{
	public long? InstitutionId { get; set; }
	public string ExternIdentifierValue { get; set; }
}
