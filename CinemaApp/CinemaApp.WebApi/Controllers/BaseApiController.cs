namespace CinemaApp.WebApi.Controllers
{
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Web.Infrastructure.Extensions;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]/")]
    public abstract class BaseApiController(
        IManagerService managerService) : ControllerBase
    {
        protected async Task<bool> IsUserManagerAsync()
        {
            string? userId = this.User.GetUserId();
            bool isManager = await managerService
                .IsUserManagerAsync(userId);

            return isManager;
        }
    }
}
