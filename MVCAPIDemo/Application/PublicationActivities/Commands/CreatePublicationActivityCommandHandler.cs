using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.PublicationActivities;
using ZUEPC.DataAccess.Models.PublicationActivity;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;

namespace ZUEPC.Application.PublicationActivities.Commands;

public class CreatePublicationActivityCommandHandler : IRequestHandler<CreatePublicationActivityCommand, CreatePublicationActivityCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationActivityData _repository;

	public CreatePublicationActivityCommandHandler(IMapper mapper, IPublicationActivityData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePublicationActivityCommandResponse> Handle(CreatePublicationActivityCommand request, CancellationToken cancellationToken)
	{
		PublicationActivityModel insertModel = _mapper.Map<PublicationActivityModel>(request);
		long newId = await _repository.InsertPublicationActivityAsync(insertModel);
		insertModel.Id = newId;
		PublicationActivity domain = _mapper.Map<PublicationActivity>(insertModel);
		return new() { Success = true, PublicationActivity = domain };
	}
}
