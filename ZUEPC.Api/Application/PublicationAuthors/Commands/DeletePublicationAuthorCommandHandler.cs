using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.PublicationAuthors;
using ZUEPC.DataAccess.Models.PublicationAuthor;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class DeletePublicationAuthorCommandHandler :
	DeleteModelWithIdCommandHandlerBase<PublicationAuthorModel, long>,
	IRequestHandler<DeletePublicationAuthorCommand, DeletePublicationAuthorCommandResponse>
{
	public DeletePublicationAuthorCommandHandler(IPublicationAuthorData repository)
	: base(repository) { }

	public async Task<DeletePublicationAuthorCommandResponse> Handle(DeletePublicationAuthorCommand request, CancellationToken cancellationToken)
	{
		return await ProcessDeleteCommandAsync
			<DeletePublicationAuthorCommand, DeletePublicationAuthorCommandResponse>(request);
	}
}
