using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Publications;
using ZUEPC.DataAccess.Models.Publication;

namespace ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;

public class UpdatePublicationExternDatabaseIdCommandHandler : IRequestHandler<UpdatePublicationExternDatabaseIdCommand, UpdatePublicationExternDatabaseIdCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPublicationExternDatabaseIdData _repository;

	public UpdatePublicationExternDatabaseIdCommandHandler(IMapper mapper, IPublicationExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<UpdatePublicationExternDatabaseIdCommandResponse> Handle(UpdatePublicationExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		var currentModel = await _repository.GetPublicationExternDbIdByIdAsync(request.Id);
		if(currentModel is null)
		{
			return new() { Success = false };
		}
		var updatedModel = _mapper.Map<PublicationExternDatabaseIdModel>(request);
		int rowsUpdated = await _repository.UpdatePublicationExternDbIdAsync(updatedModel);

		return new() { Success = rowsUpdated > 0 };
	}

}
