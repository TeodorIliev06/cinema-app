﻿namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Common;
    using ViewModels.Movie;
    using Services.Data.Contracts;

    using static Common.EntityValidationConstants.Movie;
    using static Common.ErrorMessages.Movie;

    public class MovieController(
        IMovieService movieService,
        IManagerService managerService) : BaseController(managerService)
    {
        [HttpGet]
        public async Task<IActionResult> Index(AllMoviesSearchFilterViewModel formModel)
        {
            var movies = await movieService.GetAllMoviesAsync(formModel);

            var allGenres = await movieService.GetAllGenresAsync();
            var allMoviesCount = await movieService.GetMoviesCountByFilterAsync(formModel);
            var viewModel = new AllMoviesSearchFilterViewModel
            {
                Movies = movies,
                SearchQuery = formModel.SearchQuery,
                GenreFilter = formModel.GenreFilter,
                YearFilter = formModel.YearFilter,
                AllGenres = allGenres,
                CurrentPage = formModel.CurrentPage,
                TotalPages = (int)Math.Ceiling((double)allMoviesCount /
                                               formModel.EntitiesPerPage!.Value),
            };

            return View(viewModel);
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

            bool isAdded = await movieService.AddMovieAsync(model);
            if (!isAdded)
            {
                this.ModelState.AddModelError(nameof(model.ReleaseDate), $"The release date must be in the following format: {ReleaseDateFormat}");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            var isMovieGuidValid = ValidationUtils.TryGetGuid(id, out Guid movieGuid);
            if (!isMovieGuidValid)
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

            var isMovieGuidValid = ValidationUtils.TryGetGuid(id, out Guid movieGuid);
            if (!isMovieGuidValid)
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

            var isMovieGuidValid = ValidationUtils.TryGetGuid(model.Id, out Guid movieGuid);
            if (!isMovieGuidValid)
            {
                return RedirectToAction(nameof(Index));
            }

            bool isAdded = await movieService.AddMovieToCinemasAsync(movieGuid, model);
            if (!isAdded)
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

            var isMovieGuidValid = ValidationUtils.TryGetGuid(id, out Guid movieGuid);
            if (!isMovieGuidValid)
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

            var movies = await movieService
                .GetAllMoviesAsync(new AllMoviesSearchFilterViewModel());

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

            var isMovieGuidValid = ValidationUtils.TryGetGuid(id, out Guid movieGuid);
            if (!isMovieGuidValid)
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

            var isMovieGuidValid = ValidationUtils.TryGetGuid(model.Id, out Guid movieGuid);
            if (!isMovieGuidValid)
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
