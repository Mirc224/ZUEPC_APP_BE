using ZUEPC.Application.Publications.Entities.Inputs.Common;

namespace ZUEPC.Application.PublicationAuthors.Entities.Inputs.PublicationAuthor;

public class PublicationAuthorBaseDto : PublicationPropertyBaseDto
{
	public long PersonId { get; set; }
	public long InstitutionId { get; set; }
	public double? ContributionRatio { get; set; }
	public string? Role { get; set; }
}
