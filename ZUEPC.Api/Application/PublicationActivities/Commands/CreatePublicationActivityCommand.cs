using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class CreatePublicationActivityCommand : EPCCreateCommandBase, IRequest<CreatePublicationActivityCommandResponse>
{
	public long PublicationId { get; set; }
	public string? Category { get; set; }
	public string? GovernmentGrant { get; set; }
	public int? ActivityYear { get; set; }
}
