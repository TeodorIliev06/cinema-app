namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Common;
    using ViewModels.Movie;
    using Services.Data.Contracts;

    using static Common.EntityValidationConstants.Movie;
    using static Common.ErrorMessages.Movie;
    using CinemaApp.Services.Data;

    public class MovieController(
        IMovieService movieService,
        IManagerService managerService) : BaseController(managerService)
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movies = await movieService.GetAllMoviesAsync();

            return View(movies);
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
        public async Task<IActionResult> Create(AddMovieFormModel model)
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

            bool result = await movieService.AddMovieAsync(model);
            if (result == false)
            {
                this.ModelState.AddModelError(nameof(model.ReleaseDate), $"The release date must be in the following format: {ReleaseDateFormat}");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            var movieGuid = Guid.Empty;

            bool isGuidValid = ValidationUtils.IsGuidValid(id, ref movieGuid);
            if (!isGuidValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var movie = await movieService.GetMovieDetailsByIdAsync(movieGuid);
            if (movie == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddToProgram(string? id)
        {
            bool isManager = await this.IsUserManagerAsync();
            if (!isManager)
            {
                return RedirectToAction(nameof(Index));
            }

            var movieGuid = Guid.Empty;

            bool isGuidValid = ValidationUtils.IsGuidValid(id, ref movieGuid);
            if (!isGuidValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = await movieService
                .GetAddMovieToCinemaViewModelByIdAsync(movieGuid);
            if (viewModel == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToProgram(AddMovieToCinemaViewModel model)
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

            var movieGuid = Guid.Empty;

            bool isGuidValid = ValidationUtils.IsGuidValid(model.Id, ref movieGuid);
            if (!isGuidValid)
            {
                return RedirectToAction(nameof(Index));
            }

            bool result = await movieService.AddMovieToCinemasAsync(movieGuid, model);
            if (result == false)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), "Cinema");
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

            var movieGuid = Guid.Empty;
            bool isIdValid = ValidationUtils.IsGuidValid(id, ref movieGuid);

            if (!isIdValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = await movieService
                .GetEditMovieFormModelByIdAsync(movieGuid);
            if (viewModel == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditMovieFormModel model)
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

            bool isUpdated = await movieService.EditMovieAsync(model);
            if (!isUpdated)
            {
                ModelState.AddModelError(string.Empty, EditMovieNotSuccessfulMessage);
                return View(model);
            }

            return RedirectToAction(nameof(Details), "Movie", new { id = model.Id });
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

            var movies = await movieService.GetAllMoviesAsync();

            return View(movies);
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

            var movieGuid = Guid.Empty;
            bool isIdValid = ValidationUtils.IsGuidValid(id, ref movieGuid);

            if (!isIdValid)
            {
                return RedirectToAction(nameof(Manage));
            }

            var viewModel = await movieService.GetMovieForDeleteByIdAsync(movieGuid);
            if (viewModel == null)
            {
                return RedirectToAction(nameof(Manage));
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SoftDeleteConfirmed(DeleteMovieViewModel model)
        {
            bool isManager = await this.IsUserManagerAsync();
            if (!isManager)
            {
                return RedirectToAction(nameof(Index));
            }

            var movieGuid = Guid.Empty;
            bool isIdValid = ValidationUtils.IsGuidValid(model.Id, ref movieGuid);

            if (!isIdValid)
            {
                return RedirectToAction(nameof(Manage));
            }

            bool isDeleted = await movieService.SoftDeleteMovieAsync(movieGuid);
            if (!isDeleted)
            {
                TempData["ErrorMessage"] = RemoveFromMovieNotSuccessfulMessage;
                return RedirectToAction(nameof(Delete), new { id = model.Id });
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}
