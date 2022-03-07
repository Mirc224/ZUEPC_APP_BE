using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Institutions.Queries.Institutions.Previews;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.Persons.Queries.Persons.Previews;
using ZUEPC.Application.PublicationAuthors.Commands;
using ZUEPC.Application.PublicationAuthors.Entities.Inputs.PublicationAuthor;
using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
using ZUEPC.Application.Publications.Entities.Inputs.Common;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationIdentifiers;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationNames;
using ZUEPC.Common.Extensions;
using ZUEPC.Common.Responses;
using ZUEPC.Localization;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class UpdatePublicationWithDetailsCommandHandler 
	: IRequestHandler<UpdatePublicationWithDetailsCommand, UpdatePublicationWithDetailsCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;
	private readonly IStringLocalizer<DataAnnotations> _localizer;

	public UpdatePublicationWithDetailsCommandHandler(IMapper mapper, IMediator mediator, IStringLocalizer<DataAnnotations> localizer)
	{
		_mapper = mapper;
		_mediator = mediator;
		_localizer = localizer;
	}

	public async Task<UpdatePublicationWithDetailsCommandResponse> Handle(UpdatePublicationWithDetailsCommand request, CancellationToken cancellationToken)
	{
		UpdatePublicationWithDetailsCommandResponse response = new() { Success = true };
		ICollection<Tuple<PublicationAuthorCreateDto, PersonPreview, InstitutionPreview>> authorsTuplesToInsert = 
			await GetAuthorsTuplesOrFillWithErrors(request.AuthorsToInsert, response);
		ICollection<Tuple<PublicationAuthorUpdateDto, PersonPreview, InstitutionPreview>> authorsTuplesToUpdate =
			await GetAuthorsTuplesOrFillWithErrors(request.AuthorsToUpdate, response);
		
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
		
		return new() { Success = true };
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

	private async Task<ICollection<Tuple<TActionDto, PersonPreview, InstitutionPreview>>> GetAuthorsTuplesOrFillWithErrors<TActionDto>(
		IEnumerable<TActionDto>? authors,
		ResponseBase response)
		where TActionDto : PublicationAuthorBaseDto
	{
		long personId = -1;
		long institutionId = -1;
		PersonPreview? personPreview = null;
		InstitutionPreview? institutionPreview = null;
		List<Tuple<TActionDto, PersonPreview, InstitutionPreview>> result = new();
		List<string> errorMessages = new();

		foreach (TActionDto authorDto in authors.OrEmptyIfNull())
		{
			personId = authorDto.PersonId;

			GetPersonPreviewQueryResponse personDetailsResponse = await _mediator.Send(new GetPersonPreviewQuery() { PersonId = personId });
			personPreview = personDetailsResponse.PersonPreview;
			if (!personDetailsResponse.Success)
			{
				response.Success = false;
				string errorMessage = string.Format(_localizer["PersonWithIdNotExist"].Value, personId);
				errorMessages.Add(errorMessage);
			}

			institutionId = authorDto.InstitutionId;
			GetInstitutionPreviewQueryResponse institutionDetailsResponse = await _mediator.Send(new GetInstitutionPreviewQuery() { InstitutionId = institutionId });
			institutionPreview = institutionDetailsResponse.InstitutionPreview;
			if (!institutionDetailsResponse.Success)
			{
				response.Success = false;
				string errorMessage = string.Format(_localizer["InstitutionWithIdNotExist"].Value, institutionId);
				errorMessages.Add(errorMessage);
			}
			result.Add(new Tuple<TActionDto, PersonPreview, InstitutionPreview>(authorDto, personPreview, institutionPreview));
		}
		if (errorMessages.Any())
		{
			response.ErrorMessages = response.ErrorMessages is null ? new List<string>() : response.ErrorMessages;
			List<string> responseMessages = response.ErrorMessages.ToList();
			responseMessages.AddRange(errorMessages);
			response.ErrorMessages = responseMessages;
		}
		return result;
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
}
