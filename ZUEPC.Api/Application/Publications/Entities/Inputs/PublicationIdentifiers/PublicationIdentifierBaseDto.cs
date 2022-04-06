using ZUEPC.Application.Publications.Entities.Inputs.Common;
using ZUEPC.Base.Entities.Dtos;

namespace ZUEPC.Application.Publications.Entities.Inputs.PublicationIdentifiers;

public class PublicationIdentifierBaseDto : PublicationPropertyBaseDto
{
	public string? IdentifierValue { get; set; }
	public string? IdentifierName { get; set; }
	public string? ISForm { get; set; }
}
