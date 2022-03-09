using AutoMapper;
using MediatR;
using ZUEPC.DataAccess.Data.Persons;
using ZUEPC.DataAccess.Models.Person;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, UpdatePersonCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IPersonData _repository;

	public UpdatePersonCommandHandler(IMapper mapper, IPersonData repository)
	{
		_mapper = mapper;
		_repository = repository;
	}
	public async Task<UpdatePersonCommandResponse> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
	{
		PersonModel? personRecord = await _repository.GetModelByIdAsync(request.Id);
		if (personRecord is null)
		{
			return new() { Success = false };
		}

		PersonModel personModel = _mapper.Map<PersonModel>(request);
		int rowsUpdated = await _repository.UpdateModelAsync(personModel);

		return new() { Success = rowsUpdated == 1 };
	}
}
