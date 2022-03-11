using ZUEPC.Application.Publications.Entities.Inputs.Common;

namespace ZUEPC.Application.PublicationActivities.Entities.Inputs.PublicationActivities;

public class PublicationActivityBaseDto : PublicationPropertyBaseDto
{
	public int? ActivityYear { get; set; }
	public string? Category { get; set; }
	public string? GovernmentGrant { get; set; }
}
