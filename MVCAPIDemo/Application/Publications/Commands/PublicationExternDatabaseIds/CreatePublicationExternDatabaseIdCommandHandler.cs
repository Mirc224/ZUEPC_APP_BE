using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class CreatePublicationExternDatabaseIdCommandHandler : IRequestHandler<CreatePublicationExternDatabaseIdCommand, CreatePublicationExternDatabaseIdCommandResult>
{
	private readonly IMapper _mapper;
	private readonly IPublicationExternDatabaseIdData _repository;

	public CreatePublicationExternDatabaseIdCommandHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<CreatePublicationExternDatabaseIdCommandResult> Handle(CreatePublicationExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		var insertModel = _mapper.Map<PublicationExternDatabaseIdModel>(request);
		long insertedId = await _repository.InsertPublicationExternDbIdAsync(insertModel);
		return new CreatePublicationExternDatabaseIdCommandResult() { Success = true };
	}
}
