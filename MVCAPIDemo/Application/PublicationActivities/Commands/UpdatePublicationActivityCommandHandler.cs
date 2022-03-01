using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.PublicationActivities;
using ZUEPC.DataAccess.Models.PublicationActivity;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class UpdatePublicationActivityCommandHandler : IRequestHandler<UpdatePublicationActivityCommand, UpdatePublicationActivityCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationActivityData _repository;

	public UpdatePublicationActivityCommandHandler(IMapper mapper, IPublicationActivityData repository )
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<UpdatePublicationActivityCommandResponse> Handle(UpdatePublicationActivityCommand request, CancellationToken cancellationToken)
	{
		PublicationActivityModel? currentModel = await _repository.GetPublicationActivityByIdAsync(request.Id);
		if (currentModel is null)
		{
			return new() { Success = false };
		}
		PublicationActivityModel updatedModel = _mapper.Map<PublicationActivityModel>(request);
		int rowsUpdated = await _repository.UpdatePublicationActivityAsync(updatedModel);
		return new() { Success = rowsUpdated == 1 };
	}
}
