using AutoMapper;
using MediatR;
using ZUEPC.Application.Institutions.Commands.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Commands.InstitutionNames;
using ZUEPC.Application.Institutions.Entities.Inputs.Common;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Entities.Inputs.InstitutionNames;
using ZUEPC.Common.Extensions;
using ZUEPC.Common.Services.ItemChecks;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Responses;

namespace ZUEPC.Application.Institutions.Commands.Institutions;

public class UpdateInstitutionWithDetailsCommandHandler : 
	IRequestHandler<UpdateInstitutionWithDetailsCommand, UpdateInstitutionWithDetailsCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;
	private readonly InstitutionItemCheckService _itemCheckService;

	public UpdateInstitutionWithDetailsCommandHandler(IMapper mapper, IMediator mediator, InstitutionItemCheckService itemCheckService)
	{
		_mapper = mapper;
		_mediator = mediator;
		_itemCheckService = itemCheckService;
	}

	public async Task<UpdateInstitutionWithDetailsCommandResponse> Handle(UpdateInstitutionWithDetailsCommand request, CancellationToken cancellationToken)
	{
		UpdateInstitutionWithDetailsCommandResponse response = new() { Success = true };

		await CheckIfInstitutionNamesAreValid(request.Id, request.NamesToUpdate?.Select(x => x.Id), response);
		await CheckIfInstitutionNamesAreValid(request.Id, request.NamesToDelete, response);

		await CheckIfInstitutionExternDatabaseIdsAreValid(request.Id, request.ExternDatabaseIdsToUpdate?.Select(x => x.Id), response);
		await CheckIfInstitutionExternDatabaseIdsAreValid(request.Id, request.ExternDatabaseIdsToDelete, response);

		if (!response.Success)
		{
			return response;
		}

		UpdateInstitutionCommand updateInstitutionCommand = _mapper.Map<UpdateInstitutionCommand>(request);
		bool updated = (await _mediator.Send(updateInstitutionCommand)).Success;
		if (!updated)
		{
			return new() { Success = false };
		}
		await ProcessInstitutionNamesAsync(request);

		await ProcessInstitutionExternDatabaseIdsAsync(request);

		return new() { Success = true };
	}

	private async Task CheckIfInstitutionExternDatabaseIdsAreValid(
		long institutionId, 
		IEnumerable<long>? idsToCheck, 
		UpdateInstitutionWithDetailsCommandResponse response)
	{
		await CheckIfInstitutionRelatedObjectsAreValid(
			institutionId,
			idsToCheck,
			_itemCheckService.CheckAndGetIfInstitutionExternDatabaseIdExistsAndRelatedToInstitutionAsync,
			response);
	}

	private async Task CheckIfInstitutionNamesAreValid(
		long institutionId,
		IEnumerable<long>? idsToCheck, 
		UpdateInstitutionWithDetailsCommandResponse response)
	{
		await CheckIfInstitutionRelatedObjectsAreValid(
			institutionId,
			idsToCheck,
			_itemCheckService.CheckAndGetIfInstitutionNameExistsAndRelatedToInstitutionAsync,
			response);
	}

	private async Task ProcessInstitutionExternDatabaseIdsAsync(UpdateInstitutionWithDetailsCommand request)
	{
		long institutionId = request.Id;
		foreach (long idToDelete in request.ExternDatabaseIdsToDelete.OrEmptyIfNull())
		{
			DeleteInstitutionExternDatabaseIdCommand deleteCommand = new() { Id = idToDelete };
			await _mediator.Send(deleteCommand);
		}

		await ProcessInstitutionPropertyAsync<InstitutionExternDatabaseIdUpdateDto, UpdateInstitutionExternDatabaseIdCommand>(
			request,
			request.ExternDatabaseIdsToUpdate,
			institutionId);

		await ProcessInstitutionPropertyAsync<InstitutionExternDatabaseIdCreateDto, CreateInstitutionExternDatabaseIdCommand>(
			request,
			request.ExternDatabaseIdsToInsert,
			institutionId);
	}

	private async Task ProcessInstitutionNamesAsync(UpdateInstitutionWithDetailsCommand request)
	{
		long institutionId = request.Id;
		foreach (long nameIdToDelete in request.NamesToDelete.OrEmptyIfNull())
		{
			DeleteInstitutionNameCommand deleteCommand = new() { Id = nameIdToDelete };
			await _mediator.Send(deleteCommand);
		}


		await ProcessInstitutionPropertyAsync<InstitutionNameUpdateDto,UpdateInstitutionNameCommand>(
			request,
			request.NamesToUpdate, 
			institutionId);

		await ProcessInstitutionPropertyAsync<InstitutionNameCreateDto,CreateInstitutionNameCommand>(
			request, 
			request.NamesToInsert,
			institutionId);
	}

	private async Task ProcessInstitutionPropertyAsync<TDto, TCommand>(
		UpdateInstitutionWithDetailsCommand request,
		IEnumerable<TDto>? propertyObjects,
		long institutionId)
		where TDto : InstitutionPropertyBaseDto
	{
		foreach (TDto propertyObject in propertyObjects.OrEmptyIfNull())
		{
			propertyObject.InstitutionId = institutionId;
			propertyObject.OriginSourceType = request.OriginSourceType;
			propertyObject.VersionDate = request.VersionDate;
			TCommand actionCommand = _mapper.Map<TCommand>(propertyObject);
			await _mediator.Send(actionCommand);
		}
	}

	private async Task CheckIfInstitutionRelatedObjectsAreValid<TDomain>(
		long personId,
		IEnumerable<long>? idsToCheck,
		Func<long, long, ResponseBase?, Task<TDomain?>> checkFunction,
		ResponseBase response)
		where TDomain : class, IInstitutionRelated
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
