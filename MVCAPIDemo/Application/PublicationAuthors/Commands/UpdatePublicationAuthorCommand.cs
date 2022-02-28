using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class UpdatePublicationAuthorCommand : EPCUpdateBaseCommand, IRequest<UpdatePublicationAuthorCommandResponse>
{
	public long PublicationId { get; set; }
	public long PersonId { get; set; }
	public long InstitutionId { get; set; }
	public double? ContributionRatio { get; set; }
	public string? Role { get; set; }
}
