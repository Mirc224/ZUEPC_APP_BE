using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class UpdatePublicationAuthorCommand : EPCUpdateCommandBase, IRequest<UpdatePublicationAuthorCommandResponse>
{
	public long PublicationId { get; set; }
	public long PersonId { get; set; }
	public long InstitutionId { get; set; }
	public double? ContributionRatio { get; set; }
	public string? Role { get; set; }
}
