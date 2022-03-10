using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.RelatedPublications;
using ZUEPC.DataAccess.Models.RelatedPublication;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;

namespace ZUEPC.Application.RelatedPublications.Commands;

public class CreateRelatedPublicationCommandHandler :
	EPCCreateSimpleModelCommandHandlerBase<RelatedPublication, RelatedPublicationModel>,
	IRequestHandler<CreateRelatedPublicationCommand, CreateRelatedPublicationCommandResponse>
{
	public CreateRelatedPublicationCommandHandler(IMapper mapper, IRelatedPublicationData repository)
		: base(mapper, repository) { }
	public async Task<CreateRelatedPublicationCommandResponse> Handle(CreateRelatedPublicationCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreateRelatedPublicationCommand, CreateRelatedPublicationCommandResponse>(request);
	}
}
