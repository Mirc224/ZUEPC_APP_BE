using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class CreatePublicationCommandHandler : IRequestHandler<CreatePublicationCommand, CreatePublicationCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationData _repository;

	public CreatePublicationCommandHandler(IMapper mapper, IPublicationData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<CreatePublicationCommandResponse> Handle(CreatePublicationCommand request, CancellationToken cancellationToken)
	{
		var model = _mapper.Map<PublicationModel>(request);
		model.CreatedAt = DateTime.UtcNow;
		if (request.VersionDate is null)
		{
			model.CreatedAt = DateTime.UtcNow;
		}
		var newPublicationId = await _repository.InsertPublicationAsync(model);
		var createdPublication = _mapper.Map<Publication>(model);
		createdPublication.Id = newPublicationId;

		return new CreatePublicationCommandResponse() { Success = true, CreatedPublication = createdPublication};
	}
}
