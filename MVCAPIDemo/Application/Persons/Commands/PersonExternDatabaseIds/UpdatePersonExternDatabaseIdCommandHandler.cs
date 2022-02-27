using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;

public class UpdatePersonExternDatabaseIdCommandHandler : IRequestHandler<UpdatePersonExternDatabaseIdCommand, UpdatePersonExternDatabaseIdCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonExternDatabaseIdData _repository;

	public UpdatePersonExternDatabaseIdCommandHandler(IMapper mapper, IPersonExternDatabaseIdData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<UpdatePersonExternDatabaseIdCommandResponse> Handle(UpdatePersonExternDatabaseIdCommand request, CancellationToken cancellationToken)
	{
		PersonExternDatabaseIdModel? currentModel = await _repository.GetPersonExternDatabaseIdByIdAsync(request.Id);
		if (currentModel is null)
		{
			return new() { Success = false };
		}
		PersonExternDatabaseIdModel updatedModel = _mapper.Map<PersonExternDatabaseIdModel>(request);
		int rowsUpdated = await _repository.UpdatePersonExternDatabaseIdAsync(updatedModel);
		return new() { Success = rowsUpdated == 1 };
	}
}
