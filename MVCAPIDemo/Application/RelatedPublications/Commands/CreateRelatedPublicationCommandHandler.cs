using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.RelatedPublications;
using ZUEPC.DataAccess.Models.RelatedPublication;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;

namespace ZUEPC.Application.RelatedPublications.Commands;

public class CreateRelatedPublicationCommandHandler : IRequestHandler<CreateRelatedPublicationCommand, CreateRelatedPublicationCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IRelatedPublicationData _repository;

	public CreateRelatedPublicationCommandHandler(IMapper mapper, IRelatedPublicationData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreateRelatedPublicationCommandResponse> Handle(CreateRelatedPublicationCommand request, CancellationToken cancellationToken)
	{
		RelatedPublicationModel insertModel = _mapper.Map<RelatedPublicationModel>(request);
		long insertedItemId = await _repository.InsertRelatedPublicationAsync(insertModel);
		insertModel.Id = insertedItemId;

		RelatedPublication domain = _mapper.Map<RelatedPublication>(insertModel);
		return new() { Success = true, RelatedPublication = domain };
	}
}
