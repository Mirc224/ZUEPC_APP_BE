using ZUEPC.EvidencePublication.Base.Domain.Institutions;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Publications.Entities.Details;

public class PublicationAuthorDetails
{
	public long Id { get; set; }
	public Person? Person { get; set; }
	public Institution? Institution { get; set; }
	public double? ContributionRatio { get; set; }
	public string? Role { get; set; }
}
