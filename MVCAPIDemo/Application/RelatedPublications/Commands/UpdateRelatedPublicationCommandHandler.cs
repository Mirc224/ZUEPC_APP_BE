using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.RelatedPublications;
using ZUEPC.DataAccess.Models.RelatedPublication;

namespace ZUEPC.Application.RelatedPublications.Commands;

public class UpdateRelatedPublicationCommandHandler : IRequestHandler<UpdateRelatedPublicationCommand, UpdateRelatedPublicationCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IRelatedPublicationData _repository;

	public UpdateRelatedPublicationCommandHandler(IMapper mapper, IRelatedPublicationData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<UpdateRelatedPublicationCommandResponse> Handle(UpdateRelatedPublicationCommand request, CancellationToken cancellationToken)
	{
		RelatedPublicationModel? searchedModel = await _repository.GetModelByIdAsync(request.Id);
		if (searchedModel is null)
		{
			return new() { Success = false };
		}

		RelatedPublicationModel updateModel = _mapper.Map<RelatedPublicationModel>(request);
		int rowsUpdated = await _repository.UpdateModelAsync(updateModel);
		return new() { Success = rowsUpdated > 0 };
	}
}
