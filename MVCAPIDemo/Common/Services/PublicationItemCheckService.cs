using MediatR;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Institutions.Queries.Institutions;
using ZUEPC.Application.Institutions.Queries.Institutions.Previews;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.Persons.Queries.Persons.Previews;
using ZUEPC.Application.PublicationAuthors.Queries;
using ZUEPC.Application.Publications.Commands.Publications;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Application.Publications.Queries.Publications.Previews;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.Application.RelatedPublications.Queries;
using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;
using ZUEPC.Localization;

namespace ZUEPC.Common.Services;

public class PublicationItemCheckService
{
	private readonly IMediator _mediator;
	private readonly IStringLocalizer<DataAnnotations> _localizer;

	public PublicationItemCheckService(IMediator mediator, IStringLocalizer<DataAnnotations> localizer)
	{
		_mediator = mediator;
		_localizer = localizer;
	}

	public async Task<PublicationPreview?> CheckAndGetPreviewIfPublicationExistsAsync(
		long publicationId, 
		ResponseBase? response = null)
	{
		GetPublicationPreviewQueryResponse queryResponse = 
			await _mediator.Send(new GetPublicationPreviewQuery() { PublicationId = publicationId });
		PublicationPreview? result = queryResponse.PublicationPreview;
		if (!queryResponse.Success && response != null)
		{
			string errorMessage = string.Format(_localizer["PublicationWithIdNotExist"].Value, publicationId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	public async Task<Publication?> CheckAndGetIfPublicationExistsAsync(
		long publicationId,
		ResponseBase? response = null)
	{
		GetPublicationQueryResponse queryResponse =
			await _mediator.Send(new GetPublicationQuery() { PublicationId = publicationId });
		Publication? result = queryResponse.Publication;
		if (!queryResponse.Success && response != null)
		{
			string errorMessage = string.Format(_localizer["PublicationWithIdNotExist"].Value, publicationId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	public async Task<PersonPreview?> CheckAndGetPreviewIfPersonExistsAsync(
		long personId, 
		ResponseBase? response = null)
	{
		GetPersonPreviewQueryResponse queryResponse =
			await _mediator.Send(new GetPersonPreviewQuery() { PersonId = personId});
		PersonPreview? result = queryResponse.PersonPreview;
		if (!queryResponse.Success && response != null)
		{
			string errorMessage = string.Format(_localizer["PersonWithIdNotExist"].Value, personId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	public async Task<InstitutionPreview?> CheckAndGetPreviewIfInstitutionExistsAsync(
		long institutionId, 
		ResponseBase? response = null)
	{
		GetInstitutionPreviewQueryResponse queryResponse =
			await _mediator.Send(new GetInstitutionPreviewQuery() { InstitutionId = institutionId });
		InstitutionPreview? result = queryResponse.InstitutionPreview;
		if (!queryResponse.Success && response != null)
		{
			string errorMessage = string.Format(_localizer["InstitutionWithIdNotExist"].Value, institutionId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}


	public async Task<RelatedPublication?> CheckAndGetIfRelatedPublicationExistsAndRelatedToPublicationAsync(
		long recordId,
		long publicationId,
		ResponseBase? response = null)
	{
		RelatedPublication? result = await CheckAndGetIfRelatedPublicationExistsAsync(recordId, response);
		if (response != null && 
			result != null &&
			publicationId != result.PublicationId)
		{
			result = null;
			string errorMessage = string.Format(_localizer["RelatedPublicationRecordNotMatchPublicationId"].Value, 
				recordId, 
				publicationId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	public async Task<PublicationIdentifier?> CheckAndGetIfPublicationIdentifierExistsAndRelatedToPublicationAsync(
		long recordId, 
		long publicationId, 
		ResponseBase response)
	{
		PublicationIdentifier? result = await CheckAndGetIfPublicationIdentifierExistsAsync(recordId, response);
		if (response != null &&
			result != null &&
			publicationId != result.PublicationId)
		{
			result = null;
			string errorMessage = string.Format(_localizer["PublicationIdentifierNotMatchPublicationId"].Value,
				recordId,
				publicationId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	public async Task<PublicationIdentifier?> CheckAndGetIfPublicationIdentifierExistsAsync(long recordId, ResponseBase response)
	{
		GetPublicationIdentifierQueryResponse queryResponse =
			await _mediator.Send(new GetPublicationIdentifierQuery() { PublicationIdentifierRecordId = recordId });
		PublicationIdentifier? result = queryResponse.PublicationIdentifier;
		if (!queryResponse.Success && response != null)
		{
			string errorMessage = string.Format(_localizer["PublicationIdentifierRecordNotExist"].Value, recordId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	public async Task<PublicationName?> CheckAndGetIfPublicationNameExistsAndRelatedToPublicationAsync(
		long recordId, 
		long publicationId, 
		ResponseBase response)
	{
		PublicationName? result = await CheckAndGetIfPublicationNameExistsAsync(recordId, response);
		if (response != null &&
			result != null &&
			publicationId != result.PublicationId)
		{
			result = null;
			string errorMessage = string.Format(_localizer["PublicationNameNotMatchPublicationId"].Value,
				recordId,
				publicationId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	private async Task<PublicationName?> CheckAndGetIfPublicationNameExistsAsync(
		long recordId, 
		ResponseBase response)
	{
		GetPublicationNameQueryResponse queryResponse =
			await _mediator.Send(new GetPublicationNameQuery() { PublicatioNameRecordId = recordId });
		PublicationName? result = queryResponse.PublicationName;
		if (!queryResponse.Success && response != null)
		{
			string errorMessage = string.Format(_localizer["PublicationNameRecordNotExist"].Value, recordId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	public async Task<RelatedPublication?> CheckAndGetIfRelatedPublicationExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		GetRelatedPublicationQueryResponse queryResponse =
			await _mediator.Send(new GetRelatedPublicationQuery() { RelatedPublicationRecordId = recordId });
		RelatedPublication? result = queryResponse.RelatedPublication;
		if (!queryResponse.Success && response != null)
		{
			string errorMessage = string.Format(_localizer["RelatedPublicationRecordWithIdNotExist"].Value, recordId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	public async Task<PublicationAuthor?> CheckAndGetIfPublicationAuthorExistsAndRelatedToPublicationAsync(
		long recordId,
		long publicationId,
		ResponseBase? response = null)
	{
		PublicationAuthor? result = await CheckAndGetIfPublicationAuthorExistsAsync(recordId, response);
		if (response != null &&
			result != null &&
			publicationId != result.PublicationId)
		{
			result = null;
			string errorMessage = string.Format(_localizer["PublicationAuthorRecordNotMatchPublicationId"].Value,
				recordId,
				publicationId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	public async Task<PublicationAuthor?> CheckAndGetIfPublicationAuthorExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		GetPublicationAuthorQueryResponse queryResponse =
			await _mediator.Send(new GetPublicationAuthorQuery() { PublicationAuthorRecordId = recordId });
		PublicationAuthor? result = queryResponse.PublicationAuthor;
		if (!queryResponse.Success && response != null)
		{
			string errorMessage = string.Format(_localizer["PublicationAuthorRecordNotExist"].Value, recordId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	public async Task<PublicationExternDatabaseId?> CheckAndGetIfPublicationExternDatabaseIdExistsAndRelatedToPublicationAsync(
		long recordId,
		long publicationId,
		ResponseBase? response = null)
	{
		PublicationExternDatabaseId? result = await CheckAndGetIfPublicationExternDatabaseIdExistsAsync(recordId, response);
		if (response != null &&
			result != null &&
			publicationId != result.PublicationId)
		{
			result = null;
			string errorMessage = string.Format(_localizer["PublicationExternDatabaseIdNotMatchPublicationId"].Value,
				recordId,
				publicationId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	public async Task<PublicationExternDatabaseId?> CheckAndGetIfPublicationExternDatabaseIdExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		GetPublicationExternDatabaseIdQueryResponse queryResponse =
			await _mediator.Send(new GetPublicationExternDatabaseIdQuery() { PublicationExternDatabaseIdId = recordId });
		PublicationExternDatabaseId? result = queryResponse.PublicationExternDatabaseId;
		if (!queryResponse.Success && response != null)
		{
			string errorMessage = string.Format(_localizer["PublicationExternDatabaseIdNotExist"].Value, recordId);
			ProcessErrorMessages(response, new string[] { errorMessage });
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
}
