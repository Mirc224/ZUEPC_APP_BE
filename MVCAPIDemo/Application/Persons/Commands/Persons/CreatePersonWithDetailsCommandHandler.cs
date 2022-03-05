using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Commands.PersonNames;
using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Application.Persons.Entities.Inputs.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Entities.Inputs.PersonNames;
using ZUEPC.Common.Extensions;
using ZUEPC.EvidencePublication.Base.Domain.Persons;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class CreatePersonWithDetailsCommandHandler : IRequestHandler<CreatePersonWithDetailsCommand, CreatePersonWithDetailsCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public CreatePersonWithDetailsCommandHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public async Task<CreatePersonWithDetailsCommandResponse> Handle(CreatePersonWithDetailsCommand request, CancellationToken cancellationToken)
	{
		CreatePersonCommand createPersonCommand = _mapper.Map<CreatePersonCommand>(request);
		Person createdPerson = (await _mediator.Send(createPersonCommand)).Person;
		PersonDetails responseObject = _mapper.Map<PersonDetails>(createdPerson);

		List<PersonName> personNames = new();
		foreach(PersonNameCreateDto name in request.Names.OrEmptyIfNull())
		{
			name.PersonId = createdPerson.Id;
			name.VersionDate = request.VersionDate;
			name.OriginSourceType = request.OriginSourceType;
			CreatePersonNameCommand createPersonNameCommand = _mapper.Map<CreatePersonNameCommand>(name);
			PersonName createdName = (await _mediator.Send(createPersonNameCommand)).PersonName;
			personNames.Add(createdName);
		}
		responseObject.Names = personNames;

		List<PersonExternDatabaseId> personExternIds = new();
		foreach (PersonExternDatabaseIdCreateDto externIdentifier in request.ExternDatabaseIds.OrEmptyIfNull())
		{
			externIdentifier.PersonId = createdPerson.Id;
			externIdentifier.VersionDate = request.VersionDate;
			externIdentifier.OriginSourceType = request.OriginSourceType;
			CreatePersonExternDatabaseIdCommand? createPersonIdentifierCommand = _mapper.Map<CreatePersonExternDatabaseIdCommand>(externIdentifier);
			PersonExternDatabaseId createdExternDbId = (await _mediator.Send(createPersonIdentifierCommand)).PersonExternDatabaseId;
			personExternIds.Add(createdExternDbId);
		}
		responseObject.ExternDatabaseIds = personExternIds;
		return new() { Success = true, CreatedPersonDetails = responseObject};
	}

}
