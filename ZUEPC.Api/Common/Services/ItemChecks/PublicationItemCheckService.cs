using MediatR;
using Microsoft.Extensions.Localization;
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
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.EvidencePublication.Domain.PublicationActivities;
using ZUEPC.EvidencePublication.Domain.Publications;
using ZUEPC.EvidencePublication.Domain.RelatedPublications;
using ZUEPC.EvidencePublication.PublicationAuthors;
using ZUEPC.Base.Queries;
using ZUEPC.Localization;
using ZUEPC.Base.Responses;

namespace ZUEPC.Common.Services.ItemChecks;

public class PublicationItemCheckService : EPCDomainItemsCheckServiceBase
{

	public PublicationItemCheckService(IMediator mediator, IStringLocalizer<DataAnnotations> localizer)
		: base(mediator, localizer) { }

	public async Task<PublicationPreview?> CheckAndGetPreviewIfPublicationExistsAsync(
		long publicationId, 
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<PublicationPreview,
			GetPublicationPreviewQuery,
			GetPublicationPreviewQueryResponse>(
			publicationId, 
			DataAnnotationsKeyConstants.PUBLICATION_NOT_EXIST,
			response);
	}

	public async Task<Publication?> CheckAndGetIfPublicationExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<Publication,
			GetPublicationQuery,
			GetPublicationQueryResponse>(recordId,
			DataAnnotationsKeyConstants.PUBLICATION_NOT_EXIST,
			response);
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
			DataAnnotationsKeyConstants.PERSON_NOT_EXIST,
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
			DataAnnotationsKeyConstants.INSTITUTION_NOT_EXIST,
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
			DataAnnotationsKeyConstants.PUBLICATION_ACTIVITY_NOT_MATCH_PUBLICATION_ID,
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
			GetPublicationActivityQueryResponse>(
			recordId, 
			DataAnnotationsKeyConstants.PUBLICATION_ACTIVITY_NOT_EXIST, 
			response);
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
			DataAnnotationsKeyConstants.PUBLICATION_IDENTIFIER_NOT_MATCH_PUBLICATION_ID,
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
			GetPublicationIdentifierQueryResponse>(
			recordId, 
			DataAnnotationsKeyConstants.PUBLICATION_IDENTIFIER_NOT_EXIST, 
			response);
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
			DataAnnotationsKeyConstants.PUBLICATION_NAME_NOT_MATCH_PUBLICATION_ID,
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
			GetPublicationNameQueryResponse>(
			recordId, 
			DataAnnotationsKeyConstants.PUBLICATION_NAME_NOT_EXIST, 
			response);
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
			DataAnnotationsKeyConstants.RELATED_PUBLICATION_NOT_MATCH_PUBLICATION_ID,
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
			GetRelatedPublicationQueryResponse>(
			recordId, 
			DataAnnotationsKeyConstants.RELATED_PUBLICATION_NOT_EXIST,
			response);
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
			DataAnnotationsKeyConstants.PUBLICATION_AUTHOR_NOT_MATCH_PUBLICATION_ID,
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
			GetPublicationAuthorQueryResponse>(
			recordId, 
			DataAnnotationsKeyConstants.PUBLICATION_AUTHOR_NOT_EXIST, 
			response);
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
			DataAnnotationsKeyConstants.PUBLICATION_EXTERN_DATABASE_ID_NOT_MATCH_PUBLICATION_ID,
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
			GetPublicationExternDatabaseIdQueryResponse>(
			recordId, 
			DataAnnotationsKeyConstants.PUBLICATION_EXTERN_DATABASE_ID_NOT_EXIST,
			response);
	}

	protected async Task<TDomain?> CheckAndGetIfObjectExistsAndRelatedToPublicationAsync<TDomain, TQuery, TResponse>(
		long recordId,
		long publicationId,
		string annotationNotRelatedKey,
		Func<long, ResponseBase?, Task<TDomain?>> existenceCheckFunc,
		ResponseBase? response = null)
		where TDomain : class, IPublicationRelated
		where TQuery : QueryWithIdBase<long>, new()
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
}
