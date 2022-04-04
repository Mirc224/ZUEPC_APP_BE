using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class UpdatePublicationActivityCommand : EPCUpdateCommandBase, IRequest<UpdatePublicationActivityCommandResponse>
{
	public long PublicationId { get; set; }
	public string? Category { get; set; }
	public string? GovernmentGrant { get; set; }
	public int? ActivityYear { get; set; }
}
