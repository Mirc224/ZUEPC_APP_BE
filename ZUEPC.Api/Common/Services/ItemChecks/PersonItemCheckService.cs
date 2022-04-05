using MediatR;
using Microsoft.Extensions.Localization;
using ZUEPC.Application.Persons.Queries.PersonExternDatabaseIds;
using ZUEPC.Application.Persons.Queries.PersonNames;
using ZUEPC.Base.ItemInterfaces;
using ZUEPC.EvidencePublication.Domain.Persons;
using ZUEPC.Base.Queries;
using ZUEPC.Localization;
using ZUEPC.Base.Responses;

namespace ZUEPC.Common.Services.ItemChecks;

public class PersonItemCheckService :
	EPCDomainItemsCheckServiceBase
{
	public PersonItemCheckService(IMediator mediator, IStringLocalizer<DataAnnotations> _localizer)
		: base(mediator, _localizer) { }

	public async Task<PersonName?> CheckAndGetIfPersonNameExistsAndRelatedToPersonAsync(
		long recordId,
		long personId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfObjectExistsAndRelatedToPersonAsync
			<PersonName,
			GetPersonNameQuery,
			GetPersonNameQueryResponse>(
			recordId,
			personId,
			DataAnnotationsKeyConstants.PERSON_NAME_NOT_MATCH_PERSON_ID,
			CheckAndGetIfPersonNameExistsAsync,
			response);
	}

	public async Task<PersonName?> CheckAndGetIfPersonNameExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<PersonName,
			GetPersonNameQuery,
			GetPersonNameQueryResponse>(
			recordId,
			DataAnnotationsKeyConstants.PERSON_NAME_NOT_EXIST, 
			response);
	}

	public async Task<PersonExternDatabaseId?> CheckAndGetIfPersonExternDatabaseIdExistsAndRelatedToPersonAsync(
		long recordId,
		long personId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfObjectExistsAndRelatedToPersonAsync<
			PersonExternDatabaseId,
			GetPersonExternDatabaseIdQuery,
			GetPersonExternDatabaseIdQueryResponse>(
			recordId,
			personId,
			DataAnnotationsKeyConstants.PERSON_EXTERN_DB_ID_NOT_MATCH_PERSON_ID,
			CheckAndGetIfPersonExternDatabaseIdExistsAsync,
			response);
	}

	public async Task<PersonExternDatabaseId?> CheckAndGetIfPersonExternDatabaseIdExistsAsync(
		long recordId,
		ResponseBase? response = null)
	{
		return await CheckAndGetIfSimpleObjectExistsAsync
			<PersonExternDatabaseId,
			GetPersonExternDatabaseIdQuery,
			GetPersonExternDatabaseIdQueryResponse>(
			recordId,
			DataAnnotationsKeyConstants.PERSON_EXTERN_DB_ID_NOT_EXIST, 
			response);
	}

	protected async Task<TDomain?> CheckAndGetIfObjectExistsAndRelatedToPersonAsync<TDomain, TQuery, TResponse>(
		long recordId,
		long personId,
		string annotationNotRelatedKey,
		Func<long, ResponseBase?, Task<TDomain?>> existenceCheckFunc,
		ResponseBase? response = null)
		where TDomain : class, IPersonRelated
		where TQuery : QueryWithIdBase<long>, new()
		where TResponse : ResponseWithDataBase<TDomain>
	{
		TDomain? result = await existenceCheckFunc(recordId, response);
		if (response != null &&
			result != null &&
			personId != result.PersonId)
		{
			result = null;
			string errorMessage = string.Format(_localizer[annotationNotRelatedKey].Value,
				recordId,
				personId);
			ProcessErrorMessages(response, new string[] { errorMessage });
		}
		return result;
	}
}
