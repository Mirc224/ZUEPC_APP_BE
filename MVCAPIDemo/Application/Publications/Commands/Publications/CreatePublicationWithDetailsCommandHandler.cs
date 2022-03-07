using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Institutions.Queries.Institutions.Previews;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.Persons.Queries.Persons.Previews;
using ZUEPC.Application.PublicationAuthors.Commands;
using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Application.PublicationAuthors.Entities.Inputs.PublicationAuthor;
using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Application.Publications.Entities.Inputs.Common;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationIdentifiers;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationNames;
using ZUEPC.Common.Extensions;
using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.Localization;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class CreatePublicationWithDetailsCommandHandler :
	IRequestHandler<CreatePublicationWithDetailsCommand, CreatePublicationWithDetailsCommandResponse>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;
	private readonly IStringLocalizer<DataAnnotations> _localizer;

	public CreatePublicationWithDetailsCommandHandler(
		IMapper mapper, 
		IMediator mediator, 
		IStringLocalizer<DataAnnotations> localizer)
	{
		_mapper = mapper;
		_mediator = mediator;
		_localizer = localizer;
	}

	public async Task<CreatePublicationWithDetailsCommandResponse> Handle(CreatePublicationWithDetailsCommand request, CancellationToken cancellationToken)
	{
		CreatePublicationWithDetailsCommandResponse response = new() { Success = true };
		ICollection<Tuple<PublicationAuthorCreateDto, PersonPreview, InstitutionPreview>> authorsTuples = await GetAuthorsTuplesOrFillWithErrors(request.Authors, response);

		if(!response.Success)
		{
			return response;
		}

		CreatePublicationCommand createPublicationCommand = _mapper.Map<CreatePublicationCommand>(request);
		Publication createdPublication = (await _mediator.Send(createPublicationCommand)).Publication;
		PublicationDetails responseObject = _mapper.Map<PublicationDetails>(createdPublication);
		long publicationId = createdPublication.Id;

		responseObject.Names = await ProcessPublicationNamesAsync(request, publicationId);
		responseObject.ExternDatabaseIds = await ProcessPublicationExternDatabaseIdsAsync(request, publicationId);
		responseObject.Identifiers = await ProcessPublicationIdentifiersAsync(request, publicationId);
		responseObject.Authors = await ProcessPublicationAuthorsAsync(request, authorsTuples, publicationId);

		return new() { Success = true, CreatedPublicationDetails = responseObject};
	}

	private async Task<ICollection<Tuple<PublicationAuthorCreateDto, PersonPreview, InstitutionPreview>>> GetAuthorsTuplesOrFillWithErrors(
		IEnumerable<PublicationAuthorCreateDto>? authors, 
		ResponseBase response)
	{
		long personId = -1;
		long institutionId = -1;
		PersonPreview? personPreview = null;
		InstitutionPreview? institutionPreview = null;
		List<Tuple<PublicationAuthorCreateDto, PersonPreview, InstitutionPreview>> result = new();
		List<string> errorMessages = new();
		
		foreach(PublicationAuthorCreateDto authorDto in authors.OrEmptyIfNull())
		{
			personId = authorDto.PersonId;
			
			GetPersonPreviewQueryResponse personDetailsResponse = await _mediator.Send(new GetPersonPreviewQuery() { PersonId = personId });
			personPreview = personDetailsResponse.PersonPreview;
			if(!personDetailsResponse.Success)
			{
				response.Success = false;
				string errorMessage = string.Format(_localizer["PersonWithIdNotExist"].Value, personId);
				errorMessages.Add(errorMessage);
			}

			institutionId = authorDto.InstitutionId;
			GetInstitutionPreviewQueryResponse institutionDetailsResponse = await _mediator.Send(new GetInstitutionPreviewQuery() { InstitutionId = institutionId});
			institutionPreview = institutionDetailsResponse.InstitutionPreview;
			if (!institutionDetailsResponse.Success)
			{
				response.Success = false;
				string errorMessage = string.Format(_localizer["InstitutionWithIdNotExist"].Value, institutionId);
				errorMessages.Add(errorMessage);
			}
			result.Add(new Tuple<PublicationAuthorCreateDto, PersonPreview, InstitutionPreview>(authorDto, personPreview, institutionPreview));
		}
		if(errorMessages.Any())
		{
			response.ErrorMessages = response.ErrorMessages is null ? new List<string>() : response.ErrorMessages;
			List<string> responseMessages = response.ErrorMessages.ToList();
			responseMessages.AddRange(errorMessages);
			response.ErrorMessages = responseMessages;
		}
		return result;
	}

	private async Task<ICollection<PublicationAuthorDetails>> ProcessPublicationAuthorsAsync(
		CreatePublicationWithDetailsCommand request,
		IEnumerable<Tuple<PublicationAuthorCreateDto, PersonPreview, InstitutionPreview>> itemsToProcess,
		long publicationId)
	{
		List<PublicationAuthorDetails> resultList = new();
		foreach(Tuple<PublicationAuthorCreateDto, PersonPreview, InstitutionPreview> authorTuple in itemsToProcess)
		{
			PublicationAuthorDetails createdAuthor = await ProcessPublicationAuthorAsync(
				request,
				authorTuple.Item1,
				authorTuple.Item2,
				authorTuple.Item3,
				publicationId);
			resultList.Add(createdAuthor);
		}

		return resultList;
	}

	private async Task<PublicationAuthorDetails> ProcessPublicationAuthorAsync(
		CreatePublicationWithDetailsCommand request,
		PublicationAuthorCreateDto publicationAuthorCreateDto,
		PersonPreview personPreview,
		InstitutionPreview institutionPreview,
		long publicationId)
	{

		CreatePublicationAuthorCommandResponse response = await ProcessPublicationPropertyAsync<
			CreatePublicationAuthorCommandResponse,
			PublicationAuthorCreateDto,
			CreatePublicationAuthorCommand>(request, publicationAuthorCreateDto, publicationId);
		PublicationAuthorDetails author = _mapper.Map<PublicationAuthorDetails>(response.PublicationAuthor);
		author.PersonPreview = personPreview;
		author.InstitutionPreview = institutionPreview;

		return author;
	}

	private async Task<ICollection<PublicationIdentifier>> ProcessPublicationIdentifiersAsync(CreatePublicationWithDetailsCommand request, long publicationId)
	{
		ICollection<CreatePublicationIdentifierCommandResponse> responses = await ProcessPublicationPropertiesAsync<
			CreatePublicationIdentifierCommandResponse,
			PublicationIdentifierCreateDto,
			CreatePublicationIdentifierCommand>(request, request.Identifiers, publicationId);

		return responses.Select(x => x.PublicationIdentifier).ToList();
	}

	private async Task<ICollection<PublicationExternDatabaseId>> ProcessPublicationExternDatabaseIdsAsync(CreatePublicationWithDetailsCommand request, long publicationId)
	{
		ICollection<CreatePublicationExternDatabaseIdCommandResponse> responses = await ProcessPublicationPropertiesAsync<
			CreatePublicationExternDatabaseIdCommandResponse,
			PublicationExternDatabaseIdCreateDto,
			CreatePublicationExternDatabaseIdCommand>(request, request.ExternDatabaseIds, publicationId);

		return responses.Select(x => x.PublicationExternDatabaseId).ToList();
	}

	private async Task<ICollection<PublicationName>> ProcessPublicationNamesAsync(CreatePublicationWithDetailsCommand request, long publicationId)
	{
		ICollection<CreatePublicationNameCommandResponse> responses = await ProcessPublicationPropertiesAsync<
			CreatePublicationNameCommandResponse,
			PublicationNameCreateDto,
			CreatePublicationNameCommand>(request, request.Names, publicationId);

		return responses.Select(x => x.PublicationName).ToList();
	}

	private async Task<ICollection<TResponse>> ProcessPublicationPropertiesAsync<TResponse, TCreateDto, TCommand>(
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

	private async Task<TResponse> ProcessPublicationPropertyAsync<TResponse, TCreateDto, TCommand>(
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
