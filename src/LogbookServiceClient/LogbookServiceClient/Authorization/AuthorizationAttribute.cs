using LogbookService.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Logbook.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous == true)
        {
            return;
        }

        bool isAuthenticated = context.HttpContext.User.Identity?.IsAuthenticated ?? false;
        if (isAuthenticated == false)
        {
            context.Result = new UnauthorizedResult();
        }

        SkydiverInfo? skydiverInfo = context.HttpContext.Items["SkydiverInfo"] as SkydiverInfo;
        if (skydiverInfo == null)
        {
            context.Result = new JsonResult(new { message = "Unauthorized"})
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}