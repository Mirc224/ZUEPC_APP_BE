using AutoMapper;
using MediatR;
using ZUEPC.Common.Commands;
using ZUEPC.DataAccess.Data.PublicationActivities;
using ZUEPC.DataAccess.Models.PublicationActivity;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class CreatePublicationActivityCommandHandler : 
	CreateBaseHandler,
	IRequestHandler<CreatePublicationActivityCommand, CreatePublicationActivityCommandResponse>
{
	private readonly IPublicationActivityData _repository;

	public CreatePublicationActivityCommandHandler(IMapper mapper, IPublicationActivityData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePublicationActivityCommandResponse> Handle(CreatePublicationActivityCommand request, CancellationToken cancellationToken)
	{
		PublicationActivityModel insertModel = CreateInsertModelFromRequest
			<PublicationActivityModel, CreatePublicationActivityCommand>(request);
		long insertedId = await _repository.InsertModelAsync(insertModel);

		return CreateSuccessResponseWithDataFromInsertModel
		<CreatePublicationActivityCommandResponse,
		PublicationActivity,
		PublicationActivityModel>(insertModel, insertedId);
	}
}
