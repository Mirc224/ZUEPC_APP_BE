using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.PublicationActivities.Commands;
using ZUEPC.Application.PublicationActivities.Entities.Inputs.PublicationActivities;
using ZUEPC.Application.PublicationAuthors.Commands;
using ZUEPC.Application.PublicationAuthors.Entities.Inputs.PublicationAuthor;
using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
using ZUEPC.Application.Publications.Commands.Publications.Common;
using ZUEPC.Application.Publications.Entities.Inputs.Common;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationIdentifiers;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationNames;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.RelatedPublications.Commands;
using ZUEPC.Application.RelatedPublications.Entities.Inputs.RelatedPublications;
using ZUEPC.Base.Extensions;
using ZUEPC.Common.Services.ItemChecks;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.Localization;
using ZUEPC.Base.Responses;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class UpdatePublicationWithDetailsCommandHandler :
	ActionPublicationWithDetailsCommandBaseHandler,
	IRequestHandler<UpdatePublicationWithDetailsCommand, UpdatePublicationWithDetailsCommandResponse>
{
	public UpdatePublicationWithDetailsCommandHandler(
		IMapper mapper, 
		IMediator mediator, 
		IStringLocalizer<DataAnnotations> localizer, 
		PublicationItemCheckService itemCheckService)
		:base(mapper, mediator, localizer, itemCheckService)
	{
	}

	public async Task<UpdatePublicationWithDetailsCommandResponse> Handle(UpdatePublicationWithDetailsCommand request, CancellationToken cancellationToken)
	{
		UpdatePublicationWithDetailsCommandResponse response = new() { Success = true };
		ICollection<Tuple<PublicationAuthorCreateDto, PersonPreview, InstitutionPreview>> authorsTuplesToInsert = 
			await GetAuthorsTuplesToInsertOrFillWithErrors(request.AuthorsToInsert, response);
		ICollection<Tuple<PublicationAuthorUpdateDto, PersonPreview, InstitutionPreview>> authorsTuplesToUpdate =
			await GetAuthorsTuplesWithCheckOrFillWithErrors(request, response);

		ICollection<Tuple<RelatedPublicationCreateDto, PublicationPreview>> relatedPublicationsTuplesToInsert =
			await GetRelatedPublicationsTuplesToInsertOrFillWithErrors(request.RelatedPublicationsToInsert, response);

		ICollection<Tuple<RelatedPublicationUpdateDto, PublicationPreview>> relatedPublicationsTuplesToUpdate =
			await GetRelatedPublicationsTuplesWithCheckOrFillWithErrors(request, response);

		await CheckIfPublicationNamesAreValid(request.Id,request.NamesToUpdate?.Select(x => x.Id), response);
		await CheckIfPublicationNamesAreValid(request.Id,request.NamesToDelete, response);
		
		await CheckIfPublicationIdentifiersAreValid(request.Id, request.IdentifiersToUpdate?.Select(x => x.Id),response);
		await CheckIfPublicationIdentifiersAreValid(request.Id, request.IdentifiersToDelete, response);
		
		await CheckIfPublicationExternDatabaseIdsAreValid(request.Id, request.ExternDatabaseIdsToUpdate?.Select(x => x.Id), response);
		await CheckIfPublicationExternDatabaseIdsAreValid(request.Id, request.ExternDatabaseIdsToDelete, response);

		await CheckIfPublicationActivitiesAreValid(request.Id, request.PublicationActivitiesToUpdate?.Select(x => x.Id), response);
		await CheckIfPublicationActivitiesAreValid(request.Id, request.PublicationActivitiesToDelete, response);

		if (!response.Success)
		{
			return response;
		}
		
		UpdatePublicationCommand updatePublicationCommand = _mapper.Map<UpdatePublicationCommand>(request);
		bool updated = (await _mediator.Send(updatePublicationCommand)).Success;
		if (!updated)
		{
			return new() { Success = false };
		}
		await ProcessPublicationNamesAsync(request);

		await ProcessPublicationExternDatabaseIdsAsync(request);

		await ProcessPublicationIdentifiersAsync(request);

		await ProcessPublicationAuthorsAsync(authorsTuplesToInsert, authorsTuplesToUpdate, request);
		
		await ProcessRelatedPublicationAsync(relatedPublicationsTuplesToInsert, relatedPublicationsTuplesToUpdate, request);
		
		await ProcessPublicationActivitiesAsync(request);
		
		return new() { Success = true };
	}

	private async Task CheckIfPublicationActivitiesAreValid(
		long publicationId,
		IEnumerable<long>? idsToCheck,
		UpdatePublicationWithDetailsCommandResponse response)
	{
		await CheckIfPublicationRelatedObjectsAreValid(
			publicationId,
			idsToCheck,
			_itemCheckService.CheckAndGetIfPublicationAcitivityExistsAndRelatedToPublicationAsync,
			response);
	}

	private async Task CheckIfPublicationIdentifiersAreValid(
		long publicationId,
		IEnumerable<long>? idsToCheck,
		UpdatePublicationWithDetailsCommandResponse response)
	{
		await CheckIfPublicationRelatedObjectsAreValid(
			publicationId,
			idsToCheck,
			_itemCheckService.CheckAndGetIfPublicationIdentifierExistsAndRelatedToPublicationAsync,
			response);
	}

	private async Task CheckIfPublicationNamesAreValid(
		long publicationId,
		IEnumerable<long>? idsToCheck,
		UpdatePublicationWithDetailsCommandResponse response)
	{
		await CheckIfPublicationRelatedObjectsAreValid(
			publicationId,
			idsToCheck,
			_itemCheckService.CheckAndGetIfPublicationNameExistsAndRelatedToPublicationAsync,
			response);
	}

	private async Task CheckIfPublicationExternDatabaseIdsAreValid(
		long publicationId,
		IEnumerable<long>? idsToCheck,
		UpdatePublicationWithDetailsCommandResponse response)
	{
		await CheckIfPublicationRelatedObjectsAreValid(
			publicationId,
			idsToCheck,
			_itemCheckService.CheckAndGetIfPublicationExternDatabaseIdExistsAndRelatedToPublicationAsync,
			response);
	}

	private async Task<ICollection<Tuple<PublicationAuthorUpdateDto, PersonPreview, InstitutionPreview>>> GetAuthorsTuplesWithCheckOrFillWithErrors(
		UpdatePublicationWithDetailsCommand request, 
		UpdatePublicationWithDetailsCommandResponse response)
	{
		await CheckIfUpdatedAuthorIsRelatedWithPublicationAsync(request.Id, request.AuthorsToUpdate?.Select(x => x.Id), response);
		
		await CheckIfUpdatedAuthorIsRelatedWithPublicationAsync(request.Id, request.AuthorsToDelete, response);
		
		ICollection<Tuple<PublicationAuthorUpdateDto, PersonPreview, InstitutionPreview>> authorsTuplesToUpdate =
			await GetAuthorsTuplesToInsertOrFillWithErrors(request.AuthorsToUpdate, response);
		
		return authorsTuplesToUpdate;
	}

	private async Task CheckIfUpdatedAuthorIsRelatedWithPublicationAsync(
		long publicationId, 
		IEnumerable<long>? idsToCheck, 
		UpdatePublicationWithDetailsCommandResponse response)
	{
		await CheckIfPublicationRelatedObjectsAreValid(
			publicationId,
			idsToCheck,
			_itemCheckService.CheckAndGetIfPublicationAuthorExistsAndRelatedToPublicationAsync,
			response);
	}

	private async Task<ICollection<Tuple<RelatedPublicationUpdateDto, PublicationPreview>>> GetRelatedPublicationsTuplesWithCheckOrFillWithErrors(
		UpdatePublicationWithDetailsCommand request, 
		UpdatePublicationWithDetailsCommandResponse response)
	{
		await CheckIfUpdatedRelatedPublicationIsRelatedWithPublicationAsync(
			request.Id, 
			request.RelatedPublicationsToUpdate?.Select(x => x.Id),
			response);
		
		await CheckIfUpdatedRelatedPublicationIsRelatedWithPublicationAsync(
			request.Id,
			request.RelatedPublicationsToDelete,
			response);
		
		ICollection<Tuple<RelatedPublicationUpdateDto, PublicationPreview>> relatedPublicationsTuplesToUpdate =
			await GetRelatedPublicationsTuplesToInsertOrFillWithErrors(request.RelatedPublicationsToUpdate, response);

		return relatedPublicationsTuplesToUpdate;
	}

	private async Task CheckIfUpdatedRelatedPublicationIsRelatedWithPublicationAsync(
		long publicationId,
		IEnumerable<long>? idsToCheck,
		ResponseBase response)
	{
		await CheckIfPublicationRelatedObjectsAreValid(
			publicationId, 
			idsToCheck, 
			_itemCheckService.CheckAndGetIfRelatedPublicationExistsAndRelatedToPublicationAsync, 
			response);
	}

	private async Task ProcessPublicationActivitiesAsync(
		UpdatePublicationWithDetailsCommand request)
	{
		long publicationId = request.Id;

		foreach (long idToDelete in request.PublicationActivitiesToDelete.OrEmptyIfNull())
		{
			DeletePublicationActivityCommand deleteCommand = new() { Id = idToDelete };
			await _mediator.Send(deleteCommand);
		}

		await ProcessPublicationPropertyAsync<PublicationActivityUpdateDto, UpdatePublicationActivityCommand>(
			request,
			request.PublicationActivitiesToUpdate,
			publicationId); ;

		await ProcessPublicationPropertyAsync<PublicationActivityCreateDto, CreatePublicationActivityCommand>(
			request,
			request.PublicationActivitiesToInsert,
			publicationId);
	}

	private async Task ProcessRelatedPublicationAsync(
		ICollection<Tuple<RelatedPublicationCreateDto, PublicationPreview>> relatedPublicationsTuplesToInsert, 
		ICollection<Tuple<RelatedPublicationUpdateDto, PublicationPreview>> relatedPublicationsTuplesToUpdate, 
		UpdatePublicationWithDetailsCommand request)
	{
		long publicationId = request.Id;

		foreach (long idToDelete in request.RelatedPublicationsToDelete.OrEmptyIfNull())
		{
			DeleteRelatedPublicationCommand deleteCommand = new() { Id = idToDelete };
			await _mediator.Send(deleteCommand);
		}

		await ProcessPublicationPropertyAsync<RelatedPublicationUpdateDto, UpdateRelatedPublicationCommand>(
			request,
			relatedPublicationsTuplesToUpdate.Select(x => x.Item1),
			publicationId);

		await ProcessPublicationPropertyAsync<RelatedPublicationCreateDto, CreateRelatedPublicationCommand>(
			request,
			relatedPublicationsTuplesToInsert.Select(x => x.Item1),
			publicationId);
	}

	private async Task ProcessPublicationAuthorsAsync(
		ICollection<Tuple<PublicationAuthorCreateDto, PersonPreview, InstitutionPreview>> authorsTuplesToInsert, 
		ICollection<Tuple<PublicationAuthorUpdateDto, PersonPreview, InstitutionPreview>> authorsTuplesToUpdate, 
		UpdatePublicationWithDetailsCommand request)
	{
		long publicationId = request.Id;

		foreach (long idToDelete in request.AuthorsToDelete.OrEmptyIfNull())
		{
			DeletePublicationAuthorCommand deleteCommand = new() { Id = idToDelete };
			await _mediator.Send(deleteCommand);
		}

		await ProcessPublicationPropertyAsync<PublicationAuthorUpdateDto, UpdatePublicationAuthorCommand>(
			request,
			authorsTuplesToUpdate.Select(x=> x.Item1),
			publicationId);

		await ProcessPublicationPropertyAsync<PublicationAuthorCreateDto, CreatePublicationAuthorCommand>(
			request,
			authorsTuplesToInsert.Select(x => x.Item1),
			publicationId);

	}

	private async Task ProcessPublicationExternDatabaseIdsAsync(UpdatePublicationWithDetailsCommand request)
	{
		long PublicationId = request.Id;
		foreach (long idToDelete in request.ExternDatabaseIdsToDelete.OrEmptyIfNull())
		{
			DeletePublicationExternDatabaseIdCommand deleteCommand = new() { Id = idToDelete };
			await _mediator.Send(deleteCommand);
		}

		await ProcessPublicationPropertyAsync<PublicationExternDatabaseIdUpdateDto, UpdatePublicationExternDatabaseIdCommand>(
			request,
			request.ExternDatabaseIdsToUpdate,
			PublicationId);

		await ProcessPublicationPropertyAsync<PublicationExternDatabaseIdCreateDto, CreatePublicationExternDatabaseIdCommand>(
			request,
			request.ExternDatabaseIdsToInsert,
			PublicationId);
	}

	private async Task ProcessPublicationNamesAsync(UpdatePublicationWithDetailsCommand request)
	{
		long publicationId = request.Id;
		foreach (long nameIdToDelete in request.NamesToDelete.OrEmptyIfNull())
		{
			DeletePublicationNameCommand deleteCommand = new() { Id = nameIdToDelete };
			await _mediator.Send(deleteCommand);
		}

		await ProcessPublicationPropertyAsync<PublicationNameUpdateDto, UpdatePublicationNameCommand>(
			request,
			request.NamesToUpdate,
			publicationId);

		await ProcessPublicationPropertyAsync<PublicationNameCreateDto, CreatePublicationNameCommand>(
			request,
			request.NamesToInsert,
			publicationId);
	}

	private async Task ProcessPublicationIdentifiersAsync(UpdatePublicationWithDetailsCommand request)
	{
		long publicationId = request.Id;
		foreach (long identiferIdToDelete in request.IdentifiersToDelete.OrEmptyIfNull())
		{
			DeletePublicationIdentifierCommand deleteCommand = new() { Id = identiferIdToDelete };
			await _mediator.Send(deleteCommand);
		}

		await ProcessPublicationPropertyAsync<PublicationIdentifierUpdateDto, UpdatePublicationIdentifierCommand>(
			request,
			request.IdentifiersToUpdate,
			publicationId);

		await ProcessPublicationPropertyAsync<PublicationIdentifierCreateDto, CreatePublicationIdentifierCommand>(
			request,
			request.IdentifiersToInsert,
			publicationId);
	}

	private async Task ProcessPublicationPropertyAsync<TDto, TCommand>(
		UpdatePublicationWithDetailsCommand request,
		IEnumerable<TDto>? propertyObjects,
		long PublicationId)
		where TDto : PublicationPropertyBaseDto
	{
		foreach (TDto propertyObject in propertyObjects.OrEmptyIfNull())
		{
			propertyObject.PublicationId = PublicationId;
			propertyObject.OriginSourceType = request.OriginSourceType;
			propertyObject.VersionDate = request.VersionDate;
			TCommand actionCommand = _mapper.Map<TCommand>(propertyObject);
			await _mediator.Send(actionCommand);
		}
	}

	private async Task CheckIfPublicationRelatedObjectsAreValid<TDomain>(
		long publicationId,
		IEnumerable<long>? idsToCheck,
		Func<long, long, ResponseBase?, Task<TDomain?>> checkFunction,
		ResponseBase response)
		where TDomain : class, IPublicationRelated
	{
		foreach (long recordId in idsToCheck.OrEmptyIfNull())
		{
			TDomain? publicationActivity =
				await checkFunction(recordId, publicationId, response);

			if (publicationActivity is null)
			{
				response.Success = false;
			}
		}
	}
}
