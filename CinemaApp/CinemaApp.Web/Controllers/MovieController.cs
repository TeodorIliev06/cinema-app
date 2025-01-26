namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;
    using ViewModels.Movie;

    using static Common.EntityValidationConstants.Movie;

    public class MovieController(IMovieService movieService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movies = await movieService.GetAllMoviesAsync();

            return View(movies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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

            bool isGuidValid = this.IsGuidValid(id, ref movieGuid);
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
        public async Task<IActionResult> AddToProgram(string? id)
        {
            var movieGuid = Guid.Empty;

            bool isGuidValid = this.IsGuidValid(id, ref movieGuid);
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
        public async Task<IActionResult> AddToProgram(AddMovieToCinemaViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var movieGuid = Guid.Empty;

            bool isGuidValid = this.IsGuidValid(model.Id, ref movieGuid);
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
