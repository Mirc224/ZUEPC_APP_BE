using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class UpdatePublicationIdentifierCommandHandler : IRequestHandler<UpdatePublicationIdentifierCommand, UpdatePublicationIdentifierCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationIdentifierData _repository;

	public UpdatePublicationIdentifierCommandHandler(IMapper mapper, IPublicationIdentifierData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<UpdatePublicationIdentifierCommandResponse> Handle(UpdatePublicationIdentifierCommand request, CancellationToken cancellationToken)
	{
		var updatedPublication = await _repository.GetModelByIdAsync(request.Id);
		if(updatedPublication is null)
		{
			return new() { Success = false };
		}

		var updateModel = _mapper.Map<PublicationIdentifierModel>(request);
		int updatedRows = await _repository.UpdateModelAsync(updateModel);
		return new() { Success = updatedRows > 0 };
	}
}
