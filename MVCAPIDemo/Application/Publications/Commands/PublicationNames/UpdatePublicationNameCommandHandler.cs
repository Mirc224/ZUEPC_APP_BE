using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.PublicationNames;

public class UpdatePublicationNameCommandHandler : IRequestHandler<UpdatePublicationNameCommand, UpdatePublicationNameCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationNameData _repository;

	public UpdatePublicationNameCommandHandler(IMapper mapper, IPublicationNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<UpdatePublicationNameCommandResponse> Handle(UpdatePublicationNameCommand request, CancellationToken cancellationToken)
	{
		var searchedModel = await _repository.GetPublicationNameByIdAsync(request.Id);
		if (searchedModel is null)
		{
			return new UpdatePublicationNameCommandResponse() { Success = false };
		}

		var updateModel = _mapper.Map<PublicationNameModel>(request);
		int rowsUpdated = await _repository.UpdatePublicationNameAsync(updateModel);
		return new UpdatePublicationNameCommandResponse() { Success = rowsUpdated > 0 };
	}
}
