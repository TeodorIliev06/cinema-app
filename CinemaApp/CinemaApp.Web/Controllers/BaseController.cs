namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        protected readonly IManagerService managerService;

        public BaseController(IManagerService managerService)
        {
            this.managerService = managerService;
        }

        protected async Task<bool> IsUserManagerAsync()
        {
            string? userId = this.User.GetUserId();
            bool isManager = await this.managerService
                .IsUserManagerAsync(userId);

            return isManager;
        }
    }
}
