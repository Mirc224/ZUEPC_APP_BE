using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.PublicationAuthors.Entities.Inputs.PublicationAuthor;
using ZUEPC.Application.Publications.Entities.Inputs.Common;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.RelatedPublications.Entities.Inputs.RelatedPublications;
using ZUEPC.Base.Extensions;
using ZUEPC.Base.Responses;
using ZUEPC.Common.Services.ItemChecks;
using ZUEPC.Localization;

namespace ZUEPC.Application.Publications.Commands.Publications.Common;

public abstract class ActionPublicationWithDetailsCommandBaseHandler
{
	protected readonly IMapper _mapper;
	protected readonly IMediator _mediator;
	protected readonly IStringLocalizer<DataAnnotations> _localizer;
	protected readonly PublicationItemCheckService _itemCheckService;

	public ActionPublicationWithDetailsCommandBaseHandler(IMapper mapper, IMediator mediator, IStringLocalizer<DataAnnotations> localizer, PublicationItemCheckService itemCheckService)
	{
		_mapper = mapper;
		_mediator = mediator;
		_localizer = localizer;
		_itemCheckService = itemCheckService;
	}

	protected async Task<ICollection<Tuple<TActionDto, PublicationPreview>>> GetRelatedPublicationsTuplesToInsertOrFillWithErrors<TActionDto>(
		IEnumerable<TActionDto>? relatedPublications,
		ResponseBase response)
		where TActionDto : RelatedPublicationBaseDto
	{
		long relatedPublicationId = -1;
		PublicationPreview? publicationPreview = null;
		List<Tuple<TActionDto, PublicationPreview>> result = new();

		foreach (TActionDto relatedPublicationDto in relatedPublications.OrEmptyIfNull())
		{
			relatedPublicationId = relatedPublicationDto.RelatedPublicationId;

			publicationPreview = await _itemCheckService.CheckAndGetPreviewIfPublicationExistsAsync(relatedPublicationId, response);
			if (publicationPreview is null)
			{
				response.Success = false;
				continue;
			}
			if(response.Success)
			{
				result.Add(new Tuple<TActionDto, PublicationPreview>(relatedPublicationDto, publicationPreview));
			}
		}
		return result;
	}

	protected async Task<ICollection<Tuple<TActionDto, PersonPreview, InstitutionPreview>>> GetAuthorsTuplesToInsertOrFillWithErrors<TActionDto>(
		IEnumerable<TActionDto>? authors,
		ResponseBase response)
		where TActionDto : PublicationAuthorBaseDto
	{
		long personId = -1;
		long institutionId = -1;
		PersonPreview? personPreview = null;
		InstitutionPreview? institutionPreview = null;
		List<Tuple<TActionDto, PersonPreview, InstitutionPreview>> result = new();

		foreach (TActionDto authorDto in authors.OrEmptyIfNull())
		{
			personId = authorDto.PersonId;

			personPreview = await _itemCheckService.CheckAndGetPreviewIfPersonExistsAsync(personId, response);
			if (personPreview is null)
			{
				response.Success = false;
			}

			institutionId = authorDto.InstitutionId;
			institutionPreview = await _itemCheckService.CheckAndGetPreviewIfInstitutionExistsAsync(institutionId, response);
			if (institutionPreview is null)
			{
				response.Success = false;
			}
			if(response.Success)
			{
				result.Add(new Tuple<TActionDto, PersonPreview, InstitutionPreview>(authorDto, personPreview, institutionPreview));
			}
		}
		return result;
	}

	protected void ProcessErrorMessages(ResponseBase response, IEnumerable<string> errorMessages)
	{
		if (errorMessages.Any())
		{
			response.ErrorMessages = response.ErrorMessages is null ? new List<string>() : response.ErrorMessages;
			List<string> responseMessages = response.ErrorMessages.ToList();
			responseMessages.AddRange(errorMessages);
			response.ErrorMessages = responseMessages;
		}
	}

	protected async Task<ICollection<TResponse>> ProcessPublicationPropertiesAsync<TResponse, TCreateDto, TCommand>(
		CreatePublicationWithDetailsCommand request,
		IEnumerable<TCreateDto>? propertyObjects,
		long publicationId)
		where TCreateDto : PublicationPropertyBaseDto
	{
		List<TResponse> responses = new();
		foreach (TCreateDto propertyObject in propertyObjects.OrEmptyIfNull())
		{
			TResponse createdResponse = await ProcessPublicationPropertyAsync<TResponse, TCreateDto, TCommand>(
				request,
				propertyObject,
				publicationId);
			responses.Add(createdResponse);
		}

		return responses;
	}

	protected async Task<TResponse> ProcessPublicationPropertyAsync<TResponse, TCreateDto, TCommand>(
		CreatePublicationWithDetailsCommand request,
		TCreateDto propertyObject,
		long publicationId)
		where TCreateDto : PublicationPropertyBaseDto
	{
		propertyObject.PublicationId = publicationId;
		propertyObject.OriginSourceType = request.OriginSourceType;
		propertyObject.VersionDate = request.VersionDate;
		TCommand createPropertyObjectCommand = _mapper.Map<TCommand>(propertyObject);
		TResponse createdResponse = (TResponse)await _mediator.Send(createPropertyObjectCommand);
		return createdResponse;
	}
}
