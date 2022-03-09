using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.PublicationActivities;
using ZUEPC.DataAccess.Models.PublicationActivity;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class CreatePublicationActivityCommandHandler : 
	EPCCreateSimpleModelCommandHandlerBase<PublicationActivity, PublicationActivityModel>,
	IRequestHandler<CreatePublicationActivityCommand, CreatePublicationActivityCommandResponse>
{

	public CreatePublicationActivityCommandHandler(IMapper mapper, IPublicationActivityData repository)
		: base(mapper, repository) { }

	public async Task<CreatePublicationActivityCommandResponse> Handle(CreatePublicationActivityCommand request, CancellationToken cancellationToken)
	{
		return await ProcessInsertCommandAsync<CreatePublicationActivityCommand, CreatePublicationActivityCommandResponse>(request);
	}
}
