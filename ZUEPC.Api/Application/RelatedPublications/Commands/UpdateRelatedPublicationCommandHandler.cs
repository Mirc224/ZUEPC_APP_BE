using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.RelatedPublications;
using ZUEPC.DataAccess.Models.RelatedPublication;

namespace ZUEPC.Application.RelatedPublications.Commands;

public class UpdateRelatedPublicationCommandHandler :
	UpdateSimpleModelCommandHandlerBase<RelatedPublicationModel>,
	IRequestHandler<UpdateRelatedPublicationCommand, UpdateRelatedPublicationCommandResponse>
{
	public UpdateRelatedPublicationCommandHandler(IMapper mapper, IRelatedPublicationData repository)
	: base(mapper, repository) { }

	public async Task<UpdateRelatedPublicationCommandResponse> Handle(UpdateRelatedPublicationCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
					<UpdateRelatedPublicationCommand,
					 UpdateRelatedPublicationCommandResponse>(request);
	}
}
