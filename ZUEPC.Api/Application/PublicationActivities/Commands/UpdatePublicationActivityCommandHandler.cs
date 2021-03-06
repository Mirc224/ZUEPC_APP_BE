using AutoMapper;
using MediatR;
using ZUEPC.Common.CQRS.CommandHandlers;
using ZUEPC.DataAccess.Data.PublicationActivities;
using ZUEPC.DataAccess.Models.PublicationActivity;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class UpdatePublicationActivityCommandHandler :
	UpdateModelWithIdCommandHandlerBase<IPublicationActivityData, PublicationActivityModel, long>,
	IRequestHandler<UpdatePublicationActivityCommand, UpdatePublicationActivityCommandResponse>
{

	public UpdatePublicationActivityCommandHandler(IMapper mapper, IPublicationActivityData repository )
	: base(mapper, repository) { }
	public async Task<UpdatePublicationActivityCommandResponse> Handle(UpdatePublicationActivityCommand request, CancellationToken cancellationToken)
	{
		return await ProcessUpdateCommandFromRequestAsync
			<UpdatePublicationActivityCommand,
			UpdatePublicationActivityCommandResponse>(request);
	}
}
