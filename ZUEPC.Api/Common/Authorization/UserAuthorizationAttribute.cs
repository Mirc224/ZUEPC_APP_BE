using Constants.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using ZUEPC.Base.Enums.Users;

namespace ZUEPC.Api.Common.Authorization;

public class UserAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
{
	public void OnAuthorization(AuthorizationFilterContext context)
	{
		string? userId = context.RouteData.Values["userId"].ToString();

		bool isUserAuthorized = context.HttpContext.User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == RoleType.ADMIN.ToString());
		if(isUserAuthorized)
		{
			return;
		}
	
		isUserAuthorized = context.HttpContext.User.Claims.Any(x => x.Type == CustomClaims.UserId && x.Value == userId);

		if(!isUserAuthorized)
		{
			context.Result = new ForbidResult();
		}
	}
}
