using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class DeletePublicationIdentifierCommandHandler :
	DeleteModelWithIdCommandHandlerBase<PublicationIdentifierModel, long>,
	IRequestHandler<DeletePublicationIdentifierCommand, DeletePublicationIdentifierCommandResponse>
{
	public DeletePublicationIdentifierCommandHandler(IPublicationIdentifierData repository)
	: base(repository) { }
	public async Task<DeletePublicationIdentifierCommandResponse> Handle(DeletePublicationIdentifierCommand request, CancellationToken cancellationToken)
	{
		return await ProcessDeleteCommandAsync
			<DeletePublicationIdentifierCommand, DeletePublicationIdentifierCommandResponse>(request);
	}
}
