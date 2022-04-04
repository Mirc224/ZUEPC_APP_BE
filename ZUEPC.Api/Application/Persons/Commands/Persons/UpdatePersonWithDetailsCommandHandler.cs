using AutoMapper;
using MediatR;
using ZUEPC.Application.Persons.Commands.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Commands.PersonNames;
using ZUEPC.Application.Persons.Entities.Inputs.Common;
using ZUEPC.Application.Persons.Entities.Inputs.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Entities.Inputs.PersonNames;
using ZUEPC.Common.Extensions;
using ZUEPC.Common.Services.ItemChecks;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Responses;

namespace ZUEPC.Application.Persons.Commands.Persons;

public class UpdatePersonWithDetailsCommandHandler : IRequestHandler<UpdatePersonWithDetailsCommand, UpdatePersonWithDetailsCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;
	private readonly PersonItemCheckService _itemCheckService;

	public UpdatePersonWithDetailsCommandHandler(IMapper mapper, IMediator mediator, PersonItemCheckService itemCheckService)
	{
		_mapper = mapper;
		_mediator = mediator;
		_itemCheckService = itemCheckService;
	}

	public async Task<UpdatePersonWithDetailsCommandResponse> Handle(UpdatePersonWithDetailsCommand request, CancellationToken cancellationToken)
	{
		UpdatePersonWithDetailsCommandResponse response = new() { Success = true };

		await CheckIfPersonNamesAreValid(request.Id, request.NamesToUpdate?.Select(x => x.Id), response);
		await CheckIfPersonNamesAreValid(request.Id, request.NamesToDelete, response);

		await CheckIfPersonExternDatabaseIdsAreValid(request.Id, request.ExternDatabaseIdsToUpdate?.Select(x => x.Id), response);
		await CheckIfPersonExternDatabaseIdsAreValid(request.Id, request.ExternDatabaseIdsToDelete, response);

		if (!response.Success)
		{
			return response;
		}

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

	private async Task CheckIfPersonExternDatabaseIdsAreValid(
		long personId, 
		IEnumerable<long>? idsToCheck, 
		UpdatePersonWithDetailsCommandResponse response)
	{
		await CheckIfPersonRelatedObjectsAreValid(
			personId,
			idsToCheck,
			_itemCheckService.CheckAndGetIfPersonExternDatabaseIdExistsAndRelatedToPersonAsync,
			response);
	}

	private async Task CheckIfPersonNamesAreValid(
		long personId, 
		IEnumerable<long>? idsToCheck, 
		UpdatePersonWithDetailsCommandResponse response)
	{
		await CheckIfPersonRelatedObjectsAreValid(
			personId,
			idsToCheck,
			_itemCheckService.CheckAndGetIfPersonNameExistsAndRelatedToPersonAsync,
			response);
	}

	private async Task ProcessPersonExternDatabaseIdsAsync(UpdatePersonWithDetailsCommand request)
	{
		long personId = request.Id;
		foreach (long idToDelete in request.ExternDatabaseIdsToDelete.OrEmptyIfNull())
		{
			DeletePersonExternDatabaseIdCommand deleteCommand = new() { Id = idToDelete };
			await _mediator.Send(deleteCommand);
		}

		await ProcessPersonPropertyAsync<PersonExternDatabaseIdUpdateDto, UpdatePersonExternDatabaseIdCommand>(
			request,
			request.ExternDatabaseIdsToUpdate,
			personId);

		await ProcessPersonPropertyAsync<PersonExternDatabaseIdCreateDto, CreatePersonExternDatabaseIdCommand>(
			request,
			request.ExternDatabaseIdsToInsert,
			personId);
	}

	private async Task ProcessPersonNamesAsync(UpdatePersonWithDetailsCommand request)
	{
		long personId = request.Id;
		foreach (long nameIdToDelete in request.NamesToDelete.OrEmptyIfNull())
		{
			DeletePersonNameCommand deleteCommand = new() { Id = nameIdToDelete };
			await _mediator.Send(deleteCommand);
		}

		await ProcessPersonPropertyAsync<PersonNameUpdateDto, UpdatePersonNameCommand>(
			request,
			request.NamesToUpdate,
			personId);

		await ProcessPersonPropertyAsync<PersonNameCreateDto, CreatePersonNameCommand>(
			request,
			request.NamesToInsert,
			personId);
	}

	private async Task ProcessPersonPropertyAsync<TDto, TCommand>(
		UpdatePersonWithDetailsCommand request,
		IEnumerable<TDto>? propertyObjects,
		long personId)
		where TDto : PersonPropertyBaseDto
	{
		foreach (TDto propertyObject in propertyObjects.OrEmptyIfNull())
		{
			propertyObject.PersonId = personId;
			propertyObject.OriginSourceType = request.OriginSourceType;
			propertyObject.VersionDate = request.VersionDate;
			TCommand actionCommand = _mapper.Map<TCommand>(propertyObject);
			await _mediator.Send(actionCommand);
		}
	}

	private async Task CheckIfPersonRelatedObjectsAreValid<TDomain>(
		long personId,
		IEnumerable<long>? idsToCheck,
		Func<long, long, ResponseBase?, Task<TDomain?>> checkFunction,
		ResponseBase response)
		where TDomain : class, IPersonRelated
	{
		foreach (long recordId in idsToCheck.OrEmptyIfNull())
		{
			TDomain? publicationActivity =
				await checkFunction(recordId, personId, response);

			if (publicationActivity is null)
			{
				response.Success = false;
			}
		}
	}
}
