namespace CinemaApp.Web.Controllers
{
    using Common;
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController(
        IManagerService managerService) : Controller
    {
        protected async Task<bool> IsUserManagerAsync()
        {
            string? userId = this.User.GetUserId();
            bool isManager = await managerService
                .IsUserManagerAsync(userId);

            return isManager;
        }

        protected async Task AppendUserCookieAsync()
        {
            bool isManager = await this
                .IsUserManagerAsync();

            this.HttpContext.Response.Cookies
                .Append(ApplicationConstants.IsManagerCookieName, isManager.ToString());
        }
    }
}
