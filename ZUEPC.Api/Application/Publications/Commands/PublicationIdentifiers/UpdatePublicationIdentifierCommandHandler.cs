using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class UpdatePublicationIdentifierCommandHandler :
	UpdateSimpleModelCommandHandlerBase<IPublicationIdentifierData, PublicationIdentifierModel, long>,
	IRequestHandler<UpdatePublicationIdentifierCommand, UpdatePublicationIdentifierCommandResponse>
{

	public UpdatePublicationIdentifierCommandHandler(IMapper mapper, IPublicationIdentifierData repository)
		: base(mapper, repository) { }

	public async Task<UpdatePublicationIdentifierCommandResponse> Handle(UpdatePublicationIdentifierCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdatePublicationIdentifierCommand,
			UpdatePublicationIdentifierCommandResponse>(request);
	}
}