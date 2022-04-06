using ZUEPC.Application.Publications.Entities.Inputs.Common;
using ZUEPC.Base.Entities.Dtos;

namespace ZUEPC.Application.Publications.Entities.Inputs.PublicationNames;

public class PublicationNameBaseDto : PublicationPropertyBaseDto
{
	public string Name { get; set; }
	public string? NameType { get; set; }
}
