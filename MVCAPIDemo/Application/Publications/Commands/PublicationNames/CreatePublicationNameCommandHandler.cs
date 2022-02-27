using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationNames;

public class CreatePublicationNameCommandHandler : IRequestHandler<CreatePublicationNameCommand, CreatePublicationNameCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationNameData _repository;

	public CreatePublicationNameCommandHandler(IMapper mapper, IPublicationNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePublicationNameCommandResponse> Handle(CreatePublicationNameCommand request, CancellationToken cancellationToken)
	{
		PublicationNameModel insertModel = _mapper.Map<PublicationNameModel>(request);
		long insertedItemId = await _repository.InsertPublicationNameAsync(insertModel);
		insertModel.Id = insertedItemId;
		
		PublicationName domain = _mapper.Map<PublicationName>(insertModel);
		return new CreatePublicationNameCommandResponse() { Success = true, CreatedPublicationName = domain};
	}
}
