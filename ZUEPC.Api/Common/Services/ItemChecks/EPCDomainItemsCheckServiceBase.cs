using MediatR;
using Microsoft.Extensions.Localization;
using ZUEPC.Base.Queries;
using ZUEPC.Localization;
using ZUEPC.Base.Responses;

namespace ZUEPC.Common.Services.ItemChecks;

public class EPCDomainItemsCheckServiceBase
{
	protected readonly IMediator _mediator;
	protected readonly IStringLocalizer<DataAnnotations> _localizer;
	public EPCDomainItemsCheckServiceBase(IMediator mediator, IStringLocalizer<DataAnnotations> localizer)
	{
		_mediator = mediator;
		_localizer = localizer;
	}

	protected async Task<TDomain?> CheckAndGetIfSimpleObjectExistsAsync<TDomain, TQuery, TResponse>(
		long recordId,
		string annotationKey,
		ResponseBase? response = null)
		where TQuery : QueryWithIdBase<long>, new()
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
