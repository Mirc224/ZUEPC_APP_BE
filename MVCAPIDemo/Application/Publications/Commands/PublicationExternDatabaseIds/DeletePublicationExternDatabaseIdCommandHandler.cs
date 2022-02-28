using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class DeletePublicationExternDatabaseIdCommandHandler : IRequestHandler<DeletePublicationExternDatabaseIdCommand, DeletePublicationExternDatabaseIdCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationExternDatabaseIdData _repository;

	public DeletePublicationExternDatabaseIdCommandHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<DeletePublicationExternDatabaseIdCommandResponse> Handle(DeletePublicationExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePublicationExternDbIdByIdAsync(request.Id);

		return new() { Success = true };
	}
}
