using MediatR;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Api.Application.Publications.Commands.PublicationNames;

public class DeletePublicationNamesByPublicationIdCommandHandler :
	IRequestHandler<DeletePublicationNamesByPublicationIdCommand, DeletePublicationNamesByPublicationIdCommandResponse>
{
	private readonly IPublicationNameData _repoistory;

	public DeletePublicationNamesByPublicationIdCommandHandler(IPublicationNameData repoistory)
	{
		_repoistory = repoistory;
	}
	public async Task<DeletePublicationNamesByPublicationIdCommandResponse> Handle(DeletePublicationNamesByPublicationIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repoistory.DeletePublicationNamesByPublicationIdAsync(request.PublicationId);
		return new() { Success = true };
	}
}
