using ZUEPC.DataAccess.Models.Common;

namespace ZUEPC.DataAccess.Models.PublicationAuthor;

public class PublicationAuthorModel : EPCModelBase
{
	public long PublicationId { get; set; }
	public long PersonId { get; set; }
	public long InstitutionId {get; set; }
	public double? ContributionRatio { get; set; }
	public string? Role { get; set; }
}
