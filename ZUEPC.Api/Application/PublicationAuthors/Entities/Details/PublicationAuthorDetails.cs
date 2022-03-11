using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Common.Entities;

namespace ZUEPC.Application.PublicationAuthors.Entities.Details;

public class PublicationAuthorDetails : DetailsBase
{
	public PersonPreview? PersonPreview { get; set; }
	public InstitutionPreview? InstitutionPreview { get; set; }
	public double? ContributionRatio { get; set; }
	public string? Role { get; set; }
}
