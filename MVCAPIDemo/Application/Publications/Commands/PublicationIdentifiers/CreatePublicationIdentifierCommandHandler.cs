using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationIdentifiers;

public class CreatePublicationIdentifierCommandHandler : IRequestHandler<CreatePublicationIdentifierCommand, CreatePublicationIdentifierCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationIdentifierData _repository;

	public CreatePublicationIdentifierCommandHandler(IMapper mapper, IPublicationIdentifierData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<CreatePublicationIdentifierCommandResponse> Handle(CreatePublicationIdentifierCommand request, CancellationToken cancellationToken)
	{
		PublicationIdentifierModel publicationIdentifierModel = _mapper.Map<PublicationIdentifierModel>(request);

		long newId = await _repository.InsertPublicationIdentifierAsync(publicationIdentifierModel);
		publicationIdentifierModel.Id = newId;
		PublicationIdentifier domain = _mapper.Map<PublicationIdentifier>(publicationIdentifierModel);

		return new() { Success = newId > 0, PublicationIdentifier = domain };
	}
}
