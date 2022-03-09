using MediatR;
using Microsoft.Extensions.Localization;
using System;
using ZUEPC.Application.Institutions.Entities.Previews;
using ZUEPC.Application.Institutions.Queries.Institutions.Previews;
using ZUEPC.Application.Persons.Entities.Previews;
using ZUEPC.Application.Persons.Queries.Persons.Previews;
using ZUEPC.Application.PublicationActivities.Queries;
using ZUEPC.Application.PublicationAuthors.Queries;
using ZUEPC.Application.Publications.Entities.Previews;
using ZUEPC.Application.Publications.Queries.PublicationExternDatabaseIds;
using ZUEPC.Application.Publications.Queries.PublicationIdentifiers;
using ZUEPC.Application.Publications.Queries.PublicationNames;
using ZUEPC.Application.Publications.Queries.Publications.Previews;
using ZUEPC.Application.Publications.Queries.Publictions;
using ZUEPC.Application.RelatedPublications.Queries;
using ZUEPC.Common.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Common.Interfaces;
using ZUEPC.EvidencePublication.Base.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Base.Domain.Publications;
using ZUEPC.EvidencePublication.Base.Domain.RelatedPublications;
using ZUEPC.EvidencePublication.Base.PublicationAuthors;
using ZUEPC.EvidencePublication.Base.Queries;
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
		return await CheckAndGetIfSimpleObjectExistsAsync
			<PublicationPreview,
			GetPublicationPreviewQuery,
			GetPublicationPreviewQueryResponse>(
			publicationId, 
			"PublicationWithIdNotExist",
			response);
	}

	public async Task<Publication?> CheckAndGetIfPublicationExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<Publication,
			GetPublicationQuery,
			GetPublicationQueryResponse>(recordId, "PublicationWithIdNotExist", response);
	}

	public async Task<PersonPreview?> CheckAndGetPreviewIfPersonExistsAsync(
		long personId, 
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<PersonPreview,
			GetPersonPreviewQuery,
			GetPersonPreviewQueryResponse>(
			personId,
			"PersonWithIdNotExist",
			response);
	}

	public async Task<InstitutionPreview?> CheckAndGetPreviewIfInstitutionExistsAsync(
		long institutionId, 
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<InstitutionPreview,
			GetInstitutionPreviewQuery,
			GetInstitutionPreviewQueryResponse>(
			institutionId, 
			"InstitutionWithIdNotExist",
			response);
	}

	public async Task<PublicationActivity?> CheckAndGetIfPublicationAcitivityExistsAndRelatedToPublicationAsync(
		long recordId,
		long publicationId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfObjectExistsAndRelatedToPublicationAsync
			<PublicationActivity,
			GetPublicationActivityQuery,
			GetPublicationActivityQueryResponse>(
			recordId,
			publicationId,
			"PublicationActivityNotMatchPublicationId",
			CheckAndGetIfPublicationActivityExistsAsync,
			response);
	}

	public async Task<PublicationActivity?> CheckAndGetIfPublicationActivityExistsAsync(
		long recordId, 
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<PublicationActivity,
			GetPublicationActivityQuery,
			GetPublicationActivityQueryResponse>(recordId, "PublicationActivityRecordNotExist", response);
	}

	public async Task<PublicationIdentifier?> CheckAndGetIfPublicationIdentifierExistsAndRelatedToPublicationAsync(
		long recordId, 
		long publicationId, 
		ResponseBase? response = null)
	{
		return await CheckAndGetIfObjectExistsAndRelatedToPublicationAsync
			<PublicationIdentifier,
			GetPublicationIdentifierQuery,
			GetPublicationIdentifierQueryResponse>(
			recordId,
			publicationId,
			"PublicationIdentifierNotMatchPublicationId",
			CheckAndGetIfPublicationIdentifierExistsAsync,
			response);
	}

	public async Task<PublicationIdentifier?> CheckAndGetIfPublicationIdentifierExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<PublicationIdentifier,
			GetPublicationIdentifierQuery,
			GetPublicationIdentifierQueryResponse>(recordId, "PublicationIdentifierRecordNotExist", response);
	}

	public async Task<PublicationName?> CheckAndGetIfPublicationNameExistsAndRelatedToPublicationAsync(
		long recordId, 
		long publicationId, 
		ResponseBase? response = null)
	{
		return await CheckAndGetIfObjectExistsAndRelatedToPublicationAsync
			<PublicationName,
			GetPublicationNameQuery,
			GetPublicationNameQueryResponse>(
			recordId,
			publicationId,
			"PublicationNameNotMatchPublicationId",
			CheckAndGetIfPublicationNameExistsAsync,
			response);
	}

	public async Task<PublicationName?> CheckAndGetIfPublicationNameExistsAsync(
		long recordId, 
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<PublicationName,
			GetPublicationNameQuery,
			GetPublicationNameQueryResponse>(recordId, "PublicationNameRecordNotExist", response);
	}

	public async Task<RelatedPublication?> CheckAndGetIfRelatedPublicationExistsAndRelatedToPublicationAsync(
	long recordId,
	long publicationId,
	ResponseBase? response = null)
	{
		return await CheckAndGetIfObjectExistsAndRelatedToPublicationAsync
			<RelatedPublication,
			GetRelatedPublicationQuery,
			GetRelatedPublicationQueryResponse>(
			recordId,
			publicationId,
			"RelatedPublicationNotMatchPublicationId",
			CheckAndGetIfRelatedPublicationExistsAsync,
			response);
	}

	public async Task<RelatedPublication?> CheckAndGetIfRelatedPublicationExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<RelatedPublication,
			GetRelatedPublicationQuery,
			GetRelatedPublicationQueryResponse>(recordId, "RelatedPublicationRecordWithIdNotExist", response);
	}

	public async Task<PublicationAuthor?> CheckAndGetIfPublicationAuthorExistsAndRelatedToPublicationAsync(
		long recordId,
		long publicationId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfObjectExistsAndRelatedToPublicationAsync
			<PublicationAuthor,
			GetPublicationAuthorQuery,
			GetPublicationAuthorQueryResponse>(
			recordId,
			publicationId,
			"PublicationAuthorNotMatchPublicationId",
			CheckAndGetIfPublicationAuthorExistsAsync,
			response);
	}

	public async Task<PublicationAuthor?> CheckAndGetIfPublicationAuthorExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<PublicationAuthor,
			GetPublicationAuthorQuery,
			GetPublicationAuthorQueryResponse>(recordId, "PublicationAuthorRecordNotExist", response);
	}

	public async Task<PublicationExternDatabaseId?> CheckAndGetIfPublicationExternDatabaseIdExistsAndRelatedToPublicationAsync(
		long recordId,
		long publicationId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfObjectExistsAndRelatedToPublicationAsync<PublicationExternDatabaseId,
			GetPublicationExternDatabaseIdQuery,
			GetPublicationExternDatabaseIdQueryResponse>(
			recordId,
			publicationId,
			"PublicationExternDatabaseIdNotMatchPublicationId",
			CheckAndGetIfPublicationExternDatabaseIdExistsAsync,
			response);
	}

	public async Task<PublicationExternDatabaseId?> CheckAndGetIfPublicationExternDatabaseIdExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<PublicationExternDatabaseId,
			GetPublicationExternDatabaseIdQuery,
			GetPublicationExternDatabaseIdQueryResponse>(recordId, "PublicationExternDatabaseIdNotExist", response);
	}

	protected async Task<TDomain?> CheckAndGetIfObjectExistsAndRelatedToPublicationAsync<TDomain, TQuery, TResponse>(
		long recordId,
		long publicationId,
		string annotationNotRelatedKey,
		Func<long, ResponseBase?, Task<TDomain?>> existenceCheckFunc,
		ResponseBase? response = null)
		where TDomain : class, IPublicationRelated
		where TQuery : EPCSimpleQueryBase, new()
		where TResponse : ResponseWithDataBase<TDomain>
	{
		TDomain? result = await existenceCheckFunc(recordId, response);
		if (response != null &&
			result != null &&
			publicationId != result.PublicationId)
		{
			result = null;
			string errorMessage = string.Format(_localizer[annotationNotRelatedKey].Value,
				recordId,
				publicationId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}

	protected async Task<TDomain?> CheckAndGetIfSimpleObjectExistsAsync<TDomain, TQuery, TResponse>(
		long recordId,
		string annotationKey,
		ResponseBase? response = null)
		where TQuery : EPCSimpleQueryBase, new()
		where TResponse : ResponseWithDataBase<TDomain>
	{
		TQuery simpleQuery = new TQuery() { Id = recordId };
		return await CheckAndGetIfObjectExistsAsync<TDomain, TQuery, TResponse>(
			simpleQuery, 
			recordId, 
			annotationKey, 
			response);
	}

	protected async Task<TDomain?> CheckAndGetIfObjectExistsAsync<TDomain, TQuery, TResponse>(
		TQuery query,
		long recordId,
		string annotationKey,
		ResponseBase? response = null)
		where TQuery : new()
		where TResponse : ResponseWithDataBase<TDomain>
	{
		TResponse queryResponse = (TResponse)await _mediator.Send(query);
		TDomain? result = queryResponse.Data;
		if (!queryResponse.Success && response != null)
		{
			string errorMessage = string.Format(_localizer[annotationKey].Value, recordId);
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
