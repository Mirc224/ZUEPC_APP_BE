using MediatR;
using ZUEPC.EvidencePublication.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class CreatePublicationCommand : EPCCreateCommandBase, IRequest<CreatePublicationCommandResponse>
{
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }
}
