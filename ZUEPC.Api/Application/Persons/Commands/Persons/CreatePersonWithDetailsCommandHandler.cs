using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Commands.PersonNames;
using ZUEPC.Application.Persons.Entities.Details;
using ZUEPC.Application.Persons.Entities.Inputs.Common;
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
		Person? createdPerson = (await _mediator.Send(createPersonCommand)).Data;
		if(createdPerson is null)
		{
			return new() { Success = false };
		}
		PersonDetails responseObject = _mapper.Map<PersonDetails>(createdPerson);
		long personId = createdPerson.Id;
		
		responseObject.Names = await ProcessPersonNamesAsync(request, personId);

		
		responseObject.ExternDatabaseIds = await ProcessPersonExternDatabaseIdsAsync(request, personId);
		return new() { Success = true, CreatedPersonDetails = responseObject};
	}

	private async Task<ICollection<PersonName>> ProcessPersonNamesAsync(
		CreatePersonWithDetailsCommand request, 
		long personId)
	{
		ICollection<CreatePersonNameCommandResponse> responses = await ProcessPersonPropertyAsync<
			CreatePersonNameCommandResponse,
			PersonNameCreateDto,
			CreatePersonNameCommand>(request, request.Names, personId);

		return responses.Select(x => x.Data).ToList();
	}

	private async Task<ICollection<PersonExternDatabaseId>> ProcessPersonExternDatabaseIdsAsync(
		CreatePersonWithDetailsCommand request,
		long personId)
	{
		ICollection<CreatePersonExternDatabaseIdCommandResponse> responses = await ProcessPersonPropertyAsync<
			CreatePersonExternDatabaseIdCommandResponse,
			PersonExternDatabaseIdCreateDto,
			CreatePersonExternDatabaseIdCommand>(request, request.ExternDatabaseIds, personId);

		return responses.Select(x => x.Data).ToList();
	}

	private async Task<ICollection<TResponse>> ProcessPersonPropertyAsync<TResponse, TCreateDto, TCommand>(
	CreatePersonWithDetailsCommand request,
	IEnumerable<TCreateDto>? propertyObjects,
	long personId)
	where TCreateDto : PersonPropertyBaseDto
	{
		List<TResponse> responses = new();
		foreach (TCreateDto propertyObject in propertyObjects.OrEmptyIfNull())
		{
			propertyObject.PersonId = personId;
			propertyObject.OriginSourceType = request.OriginSourceType;
			propertyObject.VersionDate = request.VersionDate;
			TCommand createPropertyObjectCommand = _mapper.Map<TCommand>(propertyObject);
			TResponse createdResponse = (TResponse)(await _mediator.Send(createPropertyObjectCommand));
			responses.Add(createdResponse);
		}
		return responses;
	}
}
