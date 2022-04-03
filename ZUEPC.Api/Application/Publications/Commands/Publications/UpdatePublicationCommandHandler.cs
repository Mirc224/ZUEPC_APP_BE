using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class UpdatePublicationCommandHandler :
	UpdateSimpleModelCommandHandlerBase<IPublicationData, PublicationModel, long>,
	IRequestHandler<UpdatePublicationCommand, UpdatePublicationCommandResponse>
{

	public UpdatePublicationCommandHandler(IMapper mapper, IPublicationData repository)
	: base(mapper, repository) { }

	public async Task<UpdatePublicationCommandResponse> Handle(UpdatePublicationCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdatePublicationCommand,
			UpdatePublicationCommandResponse>(request);
	}
}
