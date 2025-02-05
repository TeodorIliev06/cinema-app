namespace CinemaApp.Web.Infrastructure.Attributes
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using Common;

    public class ManagerOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context != null)
            {
                bool hasCookieResult =
                    context.HttpContext.Request.Cookies
                        .TryGetValue(ApplicationConstants.IsManagerCookieName, out string? isManagerStr);
                if (!hasCookieResult)
                {
                    context.Result = new BadRequestResult();
                }

                bool canParseValue = bool.TryParse(isManagerStr, out bool isManager);
                if (!canParseValue)
                {
                    context.Result = new BadRequestResult();
                }

                if (!isManager)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
