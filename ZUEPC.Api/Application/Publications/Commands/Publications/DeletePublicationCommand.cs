using MediatR;
using ZUEPC.Base.Commands;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class DeletePublicationCommand : IRequest<DeletePublicationCommandResponse>
{
	public long PublicationId { get; set; }
}
