using MediatR;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class DeletePublicationAuthorsByInstitutionIdCommand : IRequest<DeletePublicationAuthorsByInstitutionIdCommandResponse>
{
	public long InstitutionId { get; set; }
}
