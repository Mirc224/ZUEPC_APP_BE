using MediatR;
using ZUEPC.DataAccess.Data.PublicationAuthors;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class DeletePublicationAuthorsByInstitutionIdCommandHandler :
	IRequestHandler<DeletePublicationAuthorsByInstitutionIdCommand, DeletePublicationAuthorsByInstitutionIdCommandResponse>
{
	private readonly IPublicationAuthorData _repository;

	public DeletePublicationAuthorsByInstitutionIdCommandHandler(IPublicationAuthorData repository)
	{
		_repository = repository;
	}
	public async Task<DeletePublicationAuthorsByInstitutionIdCommandResponse> Handle(
		DeletePublicationAuthorsByInstitutionIdCommand request, 
		CancellationToken cancellationToken)
	{
		await _repository.DeletePublicationAuthorsByInstitutionIdAsync(request.InstitutionId);
		return new() { Success = true };
	}
}
