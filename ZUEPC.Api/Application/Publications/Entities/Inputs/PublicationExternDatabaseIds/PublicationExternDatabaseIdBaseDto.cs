using ZUEPC.Application.Publications.Entities.Inputs.Common;

namespace ZUEPC.Application.Publications.Entities.Inputs.PublicationExternDatabaseIds;

public class PublicationExternDatabaseIdBaseDto : PublicationPropertyBaseDto
{
	public string? ExternIdentifierValue { get; set; }
}
