using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZUEPC.Application.Middleware.Errors;

namespace ZUEPC.Application.Middleware;

public class ValidationFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		if (!context.ModelState.IsValid)
		{
			KeyValuePair<string, IEnumerable<string>>[] errorsInModelState = context.ModelState
				.Where(x => x.Value?.Errors.Count > 0)
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(x => x.ErrorMessage)).ToArray();
			ErrorResponse errorResponse = new();

			foreach (KeyValuePair<string, IEnumerable<string>> error in errorsInModelState)
			{
				foreach (string subError in error.Value)
				{
					ErrorModel errorModel = new()
					{
						FieldName = error.Key,
						Message = subError
					};

					errorResponse.Errors.Add(errorModel);
				}
			}
			context.Result = new BadRequestObjectResult(errorResponse);
		}

		await next();
	}
}
