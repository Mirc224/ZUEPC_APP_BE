using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class UpdatePublicationCommandHandler : IRequestHandler<UpdatePublicationCommand, UpdatePublicationCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationData _repository;

	public UpdatePublicationCommandHandler(IMapper mapper, IPublicationData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<UpdatePublicationCommandResponse> Handle(UpdatePublicationCommand request, CancellationToken cancellationToken)
	{
		var publication = await _repository.GetModelByIdAsync(request.Id);
		if(publication is null)
		{
			return new() { Success = false };
		}
		
		var publicationModel = _mapper.Map<PublicationModel>(request);
		int rowsUpdated = await _repository.UpdateModelAsync(publicationModel);
		if(rowsUpdated != 1)
		{
			return new() { Success = false };
		}
		return new() { Success = true };
	}
}
