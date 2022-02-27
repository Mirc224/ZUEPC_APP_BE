using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;
using ZUEPC.EvidencePublication.Base.Domain.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class CreatePublicationExternDatabaseIdCommandHandler : IRequestHandler<CreatePublicationExternDatabaseIdCommand, CreatePublicationExternDatabaseIdCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationExternDatabaseIdData _repository;

	public CreatePublicationExternDatabaseIdCommandHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<CreatePublicationExternDatabaseIdCommandResponse> Handle(CreatePublicationExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		PublicationExternDatabaseIdModel insertModel = _mapper.Map<PublicationExternDatabaseIdModel>(request);
		long insertedId = await _repository.InsertPublicationExternDbIdAsync(insertModel);
		insertModel.Id = insertedId;
		PublicationExternDatabaseId domain = _mapper.Map<PublicationExternDatabaseId>(insertModel);
		return new CreatePublicationExternDatabaseIdCommandResponse() { Success = true, CreatedPublicationExternDatabaseId = domain };
	}
}
