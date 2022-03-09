using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class UpdatePublicationCommandHandler :
	EPCUpdateSimpleModelCommandHandlerBase<PublicationModel>,
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
