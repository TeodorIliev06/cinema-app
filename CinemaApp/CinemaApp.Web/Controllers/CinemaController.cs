namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Web.ViewModels.Cinema;
    using Services.Data.Contracts;

    public class CinemaController(
        ICinemaService cinemaService,
        IManagerService managerService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var cinemas = await cinemaService.GetAllOrderedByLocationAsync();

            return View(cinemas);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var userId = this.User.GetUserId()!;
            bool isManager = await managerService.IsUserManagerAsync(userId);

            if (!isManager)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AddCinemaFormModel model)
        {
            var userId = this.User.GetUserId()!;
            bool isManager = await managerService.IsUserManagerAsync(userId);

            if (!isManager)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            await cinemaService.AddCinemaAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            var cinemaGuid = Guid.Empty;
            bool isIdValid = ValidationUtils.IsGuidValid(id, ref cinemaGuid);

            if (!isIdValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = await cinemaService
                .GetCinemaDetailsByIdAsync(cinemaGuid);

            if (viewModel == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Manage()
        {
            var userId = this.User.GetUserId()!;
            bool isManager = await managerService.IsUserManagerAsync(userId);

            if (!isManager)
            {
                return RedirectToAction(nameof(Index));
            }

            var cinemas = await cinemaService.GetAllOrderedByLocationAsync();

            return View(cinemas);
        }
    }
}
