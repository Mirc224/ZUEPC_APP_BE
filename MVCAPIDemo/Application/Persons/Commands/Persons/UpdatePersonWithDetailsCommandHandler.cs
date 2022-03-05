using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Commands.PersonNames;
using ZUEPC.Application.Persons.Entities.Inputs.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Entities.Inputs.PersonNames;
using ZUEPC.Common.Extensions;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class UpdatePersonWithDetailsCommandHandler : IRequestHandler<UpdatePersonWithDetailsCommand, UpdatePersonWithDetailsCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public UpdatePersonWithDetailsCommandHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public async Task<UpdatePersonWithDetailsCommandResponse> Handle(UpdatePersonWithDetailsCommand request, CancellationToken cancellationToken)
	{
		UpdatePersonCommand updatePersonCommand = _mapper.Map<UpdatePersonCommand>(request);
		bool updated = (await _mediator.Send(updatePersonCommand)).Success;
		if (!updated)
		{
			return new() { Success = false };
		}
		await ProcessPersonNamesAsync(request);
		
		await ProcessPersonExternDatabaseIdsAsync(request);

		return new() { Success = true };
	}

	private async Task ProcessPersonExternDatabaseIdsAsync(UpdatePersonWithDetailsCommand request)
	{
		long personId = request.Id;
		foreach (long idToDelete in request.ExternDatabaseIdsToDelete.OrEmptyIfNull())
		{
			DeletePersonExternDatabaseIdCommand deleteCommand = new() { Id = idToDelete };
			await _mediator.Send(deleteCommand);
		}

		foreach (PersonExternDatabaseIdUpdateDto externDbId in request.ExternDatabaseIdsToUpdate.OrEmptyIfNull())
		{
			externDbId.PersonId = personId;
			externDbId.VersionDate = request.VersionDate;
			externDbId.OriginSourceType = request.OriginSourceType;
			UpdatePersonExternDatabaseIdCommand updateCommand = _mapper.Map<UpdatePersonExternDatabaseIdCommand>(externDbId);
			await _mediator.Send(updateCommand);
		}

		foreach (PersonExternDatabaseIdCreateDto externDbId in request.ExternDatabaseIdsToInsert.OrEmptyIfNull())
		{
			externDbId.PersonId = personId;
			externDbId.VersionDate = request.VersionDate;
			externDbId.OriginSourceType = request.OriginSourceType;
			CreatePersonExternDatabaseIdCommand createPersonIdentifierCommand = _mapper.Map<CreatePersonExternDatabaseIdCommand>(externDbId);
			await _mediator.Send(createPersonIdentifierCommand);
		}
	}

	private async Task ProcessPersonNamesAsync(UpdatePersonWithDetailsCommand request)
	{
		long personId = request.Id;
		foreach (long nameIdToDelete in request.NamesToDelete.OrEmptyIfNull())
		{
			DeletePersonNameCommand deleteCommand = new() { Id = nameIdToDelete };
			await _mediator.Send(deleteCommand);
		}

		foreach (PersonNameUpdateDto nameToUpdate in request.NamesToUpdate.OrEmptyIfNull())
		{
			nameToUpdate.PersonId = personId;
			nameToUpdate.VersionDate = request.VersionDate;
			nameToUpdate.OriginSourceType = request.OriginSourceType;
			UpdatePersonNameCommand updateCommand = _mapper.Map<UpdatePersonNameCommand>(nameToUpdate);
			await _mediator.Send(updateCommand);
		}

		foreach (PersonNameCreateDto name in request.NamesToInsert.OrEmptyIfNull())
		{
			name.PersonId = personId;
			name.VersionDate = request.VersionDate;
			name.OriginSourceType = request.OriginSourceType;
			CreatePersonNameCommand createPersonNameCommand = _mapper.Map<CreatePersonNameCommand>(name);
			await _mediator.Send(createPersonNameCommand);
		}
	}
}
