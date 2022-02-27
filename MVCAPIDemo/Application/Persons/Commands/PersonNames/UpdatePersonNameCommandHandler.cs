using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.Application.Persons.Commands.PersonNames;

public class UpdatePersonNameCommandHandler : IRequestHandler<UpdatePersonNameCommand, UpdatePersonNameCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonNameData _repository;

	public UpdatePersonNameCommandHandler(IMapper mapper, IPersonNameData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<UpdatePersonNameCommandResponse> Handle(UpdatePersonNameCommand request, CancellationToken cancellationToken)
	{
		PersonNameModel? currentModel = await _repository.GetPersonNameByIdAsync(request.Id);
		if (currentModel is null)
		{
			return new() { Success = false };
		}
		PersonNameModel updatedModel = _mapper.Map<PersonNameModel>(request);
		int rowsUpdated = await _repository.UpdatePersonNameAsync(updatedModel);
		return new() { Success = rowsUpdated == 1 };
	}
}
