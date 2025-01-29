namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Web.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;

    using Common;
    using Microsoft.AspNetCore.Authorization;
    using ViewModels.Movie;
    using Services.Data.Contracts;

    using static Common.EntityValidationConstants.Movie;

    public class MovieController(
        IMovieService movieService,
        IManagerService managerService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movies = await movieService.GetAllMoviesAsync();

            return View(movies);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AddMovieFormModel model)
        {
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
            var userId = this.User.GetUserId();
            var isManager = await managerService.IsUserManagerAsync(userId);
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
            var userId = this.User.GetUserId();
            var isManager = await managerService.IsUserManagerAsync(userId);
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
    }
}
