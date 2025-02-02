﻿namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController(
        IManagerService managerService) : Controller
    {
        protected readonly IManagerService managerService = managerService;

        protected async Task<bool> IsUserManagerAsync()
        {
            string? userId = this.User.GetUserId();
            bool isManager = await this.managerService
                .IsUserManagerAsync(userId);

            return isManager;
        }
    }
}
