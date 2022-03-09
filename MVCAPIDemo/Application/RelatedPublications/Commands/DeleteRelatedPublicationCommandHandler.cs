using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.RelatedPublications;
using ZUEPC.DataAccess.Models.RelatedPublication;

namespace ZUEPC.Application.RelatedPublications.Commands;

public class DeleteRelatedPublicationCommandHandler :
	EPCDeleteSimpleModelBaseCommandHandler<RelatedPublicationModel>,
	IRequestHandler<DeleteRelatedPublicationCommand, DeleteRelatedPublicationCommandResponse>
{
	public DeleteRelatedPublicationCommandHandler(IRelatedPublicationData repository)
	: base(repository) { }

	public async Task<DeleteRelatedPublicationCommandResponse> Handle(DeleteRelatedPublicationCommand request, CancellationToken cancellationToken)
	{
		return await ProcessDeleteCommandAsync
			<DeleteRelatedPublicationCommand, DeleteRelatedPublicationCommandResponse>(request);
	}
}
