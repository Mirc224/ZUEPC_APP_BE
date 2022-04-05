using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.PublicationActivities.Commands;
using ZUEPC.Application.PublicationActivities.Entities.Inputs.PublicationActivities;
using ZUEPC.Application.PublicationAuthors.Commands;
using ZUEPC.Application.PublicationAuthors.Entities.Details;
using ZUEPC.Application.PublicationAuthors.Entities.Inputs.PublicationAuthor;
using ZUEPC.Application.Publications.Commands.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Commands.PublicationIdentifiers;
using ZUEPC.Application.Publications.Commands.PublicationNames;
using ZUEPC.Application.Publications.Commands.Publications.Common;
using ZUEPC.Application.Publications.Entities.Details;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationIdentifiers;
using ZUEPC.Application.Publications.Entities.Inputs.PublicationNames;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.RelatedPublications.Commands;
using ZUEPC.Application.RelatedPublications.Entities.Details;
using ZUEPC.Application.RelatedPublications.Entities.Inputs.RelatedPublications;
using ZUEPC.Base.Extensions;
using ZUEPC.Common.Services.ItemChecks;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Domain.Publications;
using ZUEPC.Localization;

namespace ZUEPC.Application.Publications.Commands.Publications;

public class CreatePublicationWithDetailsCommandHandler : 
	ActionPublicationWithDetailsCommandBaseHandler,
	IRequestHandler<CreatePublicationWithDetailsCommand, CreatePublicationWithDetailsCommandResponse>
{
	public CreatePublicationWithDetailsCommandHandler(
		IMapper mapper, 
		IMediator mediator, 
		IStringLocalizer<DataAnnotations> localizer,
		PublicationItemCheckService itemCheckService)
		: base(mapper, mediator, localizer, itemCheckService)
	{
	}

	public async Task<CreatePublicationWithDetailsCommandResponse> Handle(CreatePublicationWithDetailsCommand request, CancellationToken cancellationToken)
	{
		CreatePublicationWithDetailsCommandResponse response = new() { Success = true };
		ICollection<Tuple<PublicationAuthorCreateDto, PersonPreview, InstitutionPreview>> authorsTuples = 
			await GetAuthorsTuplesToInsertOrFillWithErrors(request.Authors, response);
		ICollection<Tuple<RelatedPublicationCreateDto, PublicationPreview>> relatedPublicationsTuples = 
			await GetRelatedPublicationsTuplesToInsertOrFillWithErrors(request.RelatedPublications, response);

		if (!response.Success)
		{
			return response;
		}

		CreatePublicationCommand createPublicationCommand = _mapper.Map<CreatePublicationCommand>(request);
		Publication createdPublication = (await _mediator.Send(createPublicationCommand)).Data;
		PublicationDetails responseObject = _mapper.Map<PublicationDetails>(createdPublication);
		long publicationId = createdPublication.Id;

		responseObject.Names = await ProcessPublicationNamesAsync(request, publicationId);
		responseObject.ExternDatabaseIds = await ProcessPublicationExternDatabaseIdsAsync(request, publicationId);
		responseObject.Identifiers = await ProcessPublicationIdentifiersAsync(request, publicationId);
		responseObject.Authors = await ProcessPublicationAuthorsAsync(request, authorsTuples, publicationId);
		responseObject.RelatedPublications = await ProcessRelatedPublicationsAsync(request, relatedPublicationsTuples, publicationId);
		responseObject.PublicationActivities = await ProcessPublicationActivitiesAsync(request, publicationId);

		return new() { Success = true, Data = responseObject};
	}

	private async Task<ICollection<PublicationActivity>> ProcessPublicationActivitiesAsync(CreatePublicationWithDetailsCommand request, long publicationId)
	{
		List<PublicationActivity> resultList = new();
		foreach(PublicationActivityCreateDto publicationActivityDto in request.PublicationActivities.OrEmptyIfNull())
		{
			PublicationActivity publicationActivity = await ProcessPublicationActivityAsync(request, publicationActivityDto, publicationId);
			resultList.Add(publicationActivity);
		}
		return resultList;
	}

	private async Task<PublicationActivity> ProcessPublicationActivityAsync(
		CreatePublicationWithDetailsCommand request,
		PublicationActivityCreateDto publicationActivityCreateDto,
		long publicationId)
	{

		CreatePublicationActivityCommandResponse response = await ProcessPublicationPropertyAsync<
			CreatePublicationActivityCommandResponse,
			PublicationActivityCreateDto,
			CreatePublicationActivityCommand>(request, publicationActivityCreateDto, publicationId);

		PublicationActivity publicationActivity = _mapper.Map<PublicationActivity>(response.Data);

		return publicationActivity;
	}

	private async Task<ICollection<RelatedPublicationDetails>> ProcessRelatedPublicationsAsync(
		CreatePublicationWithDetailsCommand request, 
		ICollection<Tuple<RelatedPublicationCreateDto, PublicationPreview>> relatedPublicationsTuples, 
		long publicationId)
	{
		List<RelatedPublicationDetails> resultList = new();
		foreach (Tuple<RelatedPublicationCreateDto, PublicationPreview> relatedPublicationTuple in relatedPublicationsTuples)
		{
			RelatedPublicationDetails createdRelatedPublication = await ProcessRelatedPublicationAsync(
				request,
				relatedPublicationTuple.Item1,
				relatedPublicationTuple.Item2,
				publicationId);
			resultList.Add(createdRelatedPublication);
		}

		return resultList;
	}

	private async Task<RelatedPublicationDetails> ProcessRelatedPublicationAsync(
		CreatePublicationWithDetailsCommand request,
		RelatedPublicationCreateDto relatedPublicationCreateDto,
		PublicationPreview publictionPreview,
		long publicationId)
	{

		CreateRelatedPublicationCommandResponse response = await ProcessPublicationPropertyAsync<
			CreateRelatedPublicationCommandResponse,
			RelatedPublicationCreateDto,
			CreateRelatedPublicationCommand>(request, relatedPublicationCreateDto, publicationId);
		
		RelatedPublicationDetails relatedPublication = _mapper.Map<RelatedPublicationDetails>(response.Data);
		relatedPublication.RelatedPublication = publictionPreview;

		return relatedPublication;
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
		PublicationAuthorDetails author = _mapper.Map<PublicationAuthorDetails>(response.Data);
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

		return responses.Select(x => x.Data).ToList();
	}

	private async Task<ICollection<PublicationExternDatabaseId>> ProcessPublicationExternDatabaseIdsAsync(CreatePublicationWithDetailsCommand request, long publicationId)
	{
		ICollection<CreatePublicationExternDatabaseIdCommandResponse> responses = await ProcessPublicationPropertiesAsync<
			CreatePublicationExternDatabaseIdCommandResponse,
			PublicationExternDatabaseIdCreateDto,
			CreatePublicationExternDatabaseIdCommand>(request, request.ExternDatabaseIds, publicationId);

		return responses.Select(x => x.Data).ToList();
	}

	private async Task<ICollection<PublicationName>> ProcessPublicationNamesAsync(CreatePublicationWithDetailsCommand request, long publicationId)
	{
		ICollection<CreatePublicationNameCommandResponse> responses = await ProcessPublicationPropertiesAsync<
			CreatePublicationNameCommandResponse,
			PublicationNameCreateDto,
			CreatePublicationNameCommand>(request, request.Names, publicationId);

		return responses.Select(x => x.Data).ToList();
	}
}
