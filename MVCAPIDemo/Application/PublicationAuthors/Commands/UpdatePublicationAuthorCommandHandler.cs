using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.PublicationAuthors;
using ZUEPC.DataAccess.Models.PublicationAuthor;

namespace ZUEPC.Application.PublicationAuthors.Commands;

public class UpdatePublicationAuthorCommandHandler : 
	IRequestHandler<
		UpdatePublicationAuthorCommand, 
		UpdatePublicationAuthorCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationAuthorData _repository;

	public UpdatePublicationAuthorCommandHandler(IMapper mapper, IPublicationAuthorData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<UpdatePublicationAuthorCommandResponse> Handle(UpdatePublicationAuthorCommand request, CancellationToken cancellationToken)
	{
		PublicationAuthorModel? currentModel = await _repository.GetModelByIdAsync(request.Id);
		if (currentModel is null)
		{
			return new() { Success = false };
		}
		PublicationAuthorModel updatedModel = _mapper.Map<PublicationAuthorModel>(request);
		int rowsUpdated = await _repository.UpdateModelAsync(updatedModel);
		return new() { Success = rowsUpdated == 1 };
	}
}
