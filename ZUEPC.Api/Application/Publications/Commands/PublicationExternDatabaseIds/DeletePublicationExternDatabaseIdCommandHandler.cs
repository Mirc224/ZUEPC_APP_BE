using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class DeletePublicationExternDatabaseIdCommandHandler :
	DeleteModelWithIdCommandHandlerBase<PublicationExternDatabaseIdModel, long>,
	IRequestHandler<DeletePublicationExternDatabaseIdCommand, DeletePublicationExternDatabaseIdCommandResponse>
{
	public DeletePublicationExternDatabaseIdCommandHandler(IPublicationExternDatabaseIdData repository)
	: base(repository) { }

	public async Task<DeletePublicationExternDatabaseIdCommandResponse> Handle(DeletePublicationExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		return await ProcessDeleteCommandAsync
			<DeletePublicationExternDatabaseIdCommand, DeletePublicationExternDatabaseIdCommandResponse>(request);
	}
}
