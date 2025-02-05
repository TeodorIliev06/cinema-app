namespace CinemaApp.Web.Controllers
{
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Contracts;
    using Web.ViewModels.Cinema;
    using static Common.ErrorMessages.Cinema;

    public class CinemaController(
        ICinemaService cinemaService,
        IManagerService managerService) : BaseController(managerService)
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
            bool isManager = await this.IsUserManagerAsync();
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
            bool isManager = await this.IsUserManagerAsync();
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

            await this.AppendUserCookieAsync();

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Manage()
        {
            bool isManager = await this.IsUserManagerAsync();
            if (!isManager)
            {
                return RedirectToAction(nameof(Index));
            }

            var cinemas = await cinemaService.GetAllOrderedByLocationAsync();

            return View(cinemas);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string? id)
        {
            bool isManager = await this.IsUserManagerAsync();
            if (!isManager)
            {
                //TODO: Implement notifications and warnings for error messages
                return RedirectToAction(nameof(Index));
            }

            var cinemaGuid = Guid.Empty;
            bool isIdValid = ValidationUtils.IsGuidValid(id, ref cinemaGuid);

            if (!isIdValid)
            {
                return RedirectToAction(nameof(Manage));
            }

            var viewModel = await cinemaService
                .GetCinemaForEditByIdAsync(cinemaGuid);
            if (viewModel == null)
            {
                return RedirectToAction(nameof(Manage));
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditCinemaFormModel model)
        {
            bool isManager = await this.IsUserManagerAsync();
            if (!isManager)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isUpdated = await cinemaService.EditCinemaAsync(model);
            if (!isUpdated)
            {
                ModelState.AddModelError(string.Empty, EditCinemaNotSuccessfulMessage);
                return View(model);
            }

            return RedirectToAction(nameof(Details), "Cinema", new { id = model.Id });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string? id)
        {
            bool isManager = await this.IsUserManagerAsync();
            if (!isManager)
            {
                return RedirectToAction(nameof(Index));
            }

            var cinemaGuid = Guid.Empty;
            bool isIdValid = ValidationUtils.IsGuidValid(id, ref cinemaGuid);

            if (!isIdValid)
            {
                return RedirectToAction(nameof(Manage));
            }

            var viewModel = await cinemaService.GetCinemaForDeleteByIdAsync(cinemaGuid);
            if (viewModel == null)
            {
                return RedirectToAction(nameof(Manage));
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SoftDeleteConfirmed(DeleteCinemaViewModel model)
        {
            bool isManager = await this.IsUserManagerAsync();
            if (!isManager)
            {
                return RedirectToAction(nameof(Index));
            }

            var cinemaGuid = Guid.Empty;
            bool isIdValid = ValidationUtils.IsGuidValid(model.Id, ref cinemaGuid);

            if (!isIdValid)
            {
                return RedirectToAction(nameof(Manage));
            }

            bool isDeleted = await cinemaService.SoftDeleteCinemaAsync(cinemaGuid);
            if (!isDeleted)
            {
                TempData["ErrorMessage"] = RemoveFromCinemaNotSuccessfulMessage;
                return RedirectToAction(nameof(Delete), new { id = model.Id });
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}
