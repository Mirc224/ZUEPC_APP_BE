using MediatR;
using ZUEPC.Common.CQRS.Commands;
using ZUEPC.DataAccess.Data.PublicationActivities;
using ZUEPC.DataAccess.Models.PublicationActivity;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class DeletePublicationActivityCommandHandler :
	EPCDeleteSimpleBaseCommandHandler<PublicationActivityModel>,
	IRequestHandler<DeletePublicationActivityCommand, DeletePublicationActivityCommandResponse>
{

	public DeletePublicationActivityCommandHandler(IPublicationActivityData repository)
	: base(repository) { }

	public async Task<DeletePublicationActivityCommandResponse> Handle(DeletePublicationActivityCommand request, CancellationToken cancellationToken)
	{
		return await ProcessDeleteCommandAsync
			<DeletePublicationActivityCommand, DeletePublicationActivityCommandResponse>(request);
	}
}
