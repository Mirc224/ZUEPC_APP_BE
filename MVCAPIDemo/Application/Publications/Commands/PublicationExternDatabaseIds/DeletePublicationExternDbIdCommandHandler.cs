using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class DeletePublicationExternDbIdCommandHandler : IRequestHandler<DeletePublicationExternDbIdCommand, DeletePublicationExternDbIdCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationExternDatabaseIdData _repository;

	public DeletePublicationExternDbIdCommandHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<DeletePublicationExternDbIdCommandResponse> Handle(DeletePublicationExternDbIdCommand request, CancellationToken cancellationToken)
	{
		int rowsDeleted = await _repository.DeletePublicationExternDbIdByIdAsync(request.Id);

		return new() { Success = true };
	}
}
