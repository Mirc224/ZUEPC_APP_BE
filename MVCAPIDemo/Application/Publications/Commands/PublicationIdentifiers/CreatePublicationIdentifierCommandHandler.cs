using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

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
		var publicationIdentifierModel = _mapper.Map<PublicationIdentifierModel>(request);

		long newId = await _repository.InsertPublicationIdentifierAsync(publicationIdentifierModel);
		return new() { Success = newId > 0 };
	}
}
