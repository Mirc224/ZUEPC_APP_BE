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
		PublicationIdentifierModel insertModel = _mapper.Map<PublicationIdentifierModel>(request);
		insertModel.CreatedAt = DateTime.UtcNow;
		if (request.VersionDate is null)
		{
			insertModel.CreatedAt = DateTime.UtcNow;
		}
		long newId = await _repository.InsertModelAsync(insertModel);
		insertModel.Id = newId;
		PublicationIdentifier domain = _mapper.Map<PublicationIdentifier>(insertModel);

		return new() { Success = newId > 0, Data = domain };
	}
}
