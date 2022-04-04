using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class CreatePublicationCommand : EPCCreateCommandBase, IRequest<CreatePublicationCommandResponse>
{
	public string? DocumentType { get; set; }
	public int? PublishYear { get; set; }
}
