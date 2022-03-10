using MediatR;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Institutions.Queries.InstitutionExternDatabaseIds;
using ZUEPC.Application.Institutions.Queries.InstitutionNames;
using ZUEPC.Responses;
using ZUEPC.EvidencePublication.Base.Domain.Common.Interfaces;
using ZUEPC.EvidencePublication.Base.Domain.Institutions;
using ZUEPC.EvidencePublication.Base.Queries;
using ZUEPC.Localization;

namespace ZUEPC.Common.Services.ItemChecks;

public class InstitutionItemCheckService :
	EPCDomainItemsCheckServiceBase
{
	public InstitutionItemCheckService(IMediator mediator, IStringLocalizer<DataAnnotations> _localizer)
		: base(mediator, _localizer) { }

	public async Task<InstitutionName?> CheckAndGetIfInstitutionNameExistsAndRelatedToInstitutionAsync(
		long recordId,
		long InstitutionId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfObjectExistsAndRelatedToInstitutionAsync
			<InstitutionName,
			GetInstitutionNameQuery,
			GetInstitutionNameQueryResponse>(
			recordId,
			InstitutionId,
			DataAnnotationsKeyConstants.INSTITUTION_NAME_NOT_MATCH_INSTITUTION_ID,
			CheckAndGetIfInstitutionNameExistsAsync,
			response);
	}

	public async Task<InstitutionName?> CheckAndGetIfInstitutionNameExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<InstitutionName,
			GetInstitutionNameQuery,
			GetInstitutionNameQueryResponse>(
			recordId,
			DataAnnotationsKeyConstants.INSTITUTION_NAME_NOT_EXIST,
			response);
	}

	public async Task<InstitutionExternDatabaseId?> CheckAndGetIfInstitutionExternDatabaseIdExistsAndRelatedToInstitutionAsync(
		long recordId,
		long InstitutionId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfObjectExistsAndRelatedToInstitutionAsync<
			InstitutionExternDatabaseId,
			GetInstitutionExternDatabaseIdQuery,
			GetInstitutionExternDatabaseIdQueryResponse>(
			recordId,
			InstitutionId,
			DataAnnotationsKeyConstants.INSTITUTION_EXTERN_DB_ID_NOT_MATCH_INSTITUTION_ID,
			CheckAndGetIfInstitutionExternDatabaseIdExistsAsync,
			response);
	}

	public async Task<InstitutionExternDatabaseId?> CheckAndGetIfInstitutionExternDatabaseIdExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<InstitutionExternDatabaseId,
			GetInstitutionExternDatabaseIdQuery,
			GetInstitutionExternDatabaseIdQueryResponse>(
			recordId,
			DataAnnotationsKeyConstants.INSTITUTION_EXTERN_DB_ID_NOT_EXIST,
			response);
	}

	protected async Task<TDomain?> CheckAndGetIfObjectExistsAndRelatedToInstitutionAsync<TDomain, TQuery, TResponse>(
		long recordId,
		long institutionId,
		string annotationNotRelatedKey,
		Func<long, ResponseBase?, Task<TDomain?>> existenceCheckFunc,
		ResponseBase? response = null)
		where TDomain : class, IInstitutionRelated
		where TQuery : EPCSimpleQueryBase, new()
		where TResponse : ResponseWithDataBase<TDomain>
	{
		TDomain? result = await existenceCheckFunc(recordId, response);
		if (response != null &&
			result != null &&
			institutionId != result.InstitutionId)
		{
			result = null;
			string errorMessage = string.Format(_localizer[annotationNotRelatedKey].Value,
				recordId,
				institutionId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}
}
