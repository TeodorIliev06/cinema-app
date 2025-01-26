namespace CinemaApp.Web.Controllers
{
    using System.Globalization;

    using Data;
    using Data.Models;
    using ViewModels.Movie;
    using Web.ViewModels.Cinema;
    using Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using static Common.EntityValidationConstants.Movie;
    using static Common.EntityValidationMessages.Cinema;

    public class MovieController(CinemaDbContext dbContext, IMovieService movieService) : BaseController
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

            var movie =  await dbContext
                .Movies
                .FirstOrDefaultAsync(m => m.Id == movieGuid);

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

            var movie = await dbContext
                .Movies
                .FirstOrDefaultAsync(m => m.Id == movieGuid);

            if (movie == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new AddMovieToCinemaViewModel()
            {
                Id = id!,
                Title = movie.Title,
                Cinemas = await dbContext
                    .Cinemas
                    .Include(c => c.CinemaMovies)
                    .ThenInclude(cm => cm.Movie)
                    .Select(c => new CinemaCheckBoxItemDto()
                    {
                        Id = c.Id.ToString(),
                        Name = c.Name,
                        Location = c.Location,
                        IsSelected = c.CinemaMovies
                            .Any(cm => 
                                cm.Movie.Id == movieGuid &&
                                cm.IsDeleted == false)
                    })
                    .ToListAsync()
            };

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

            var movie = await dbContext
                .Movies
                .FirstOrDefaultAsync(m => m.Id == movieGuid);

            if (movie == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var entitiesToAdd = new List<CinemaMovie>();
            foreach (var viewModel in model.Cinemas)
            {
                var cinemaGuid = Guid.Empty;
                bool isCinemaGuidValid = this.IsGuidValid(viewModel.Id, ref cinemaGuid);

                if (!isCinemaGuidValid)
                {
                    this.ModelState.AddModelError(String.Empty, InvalidCinemaIdMessage);
                    return View(model);
                }

                var cinema = await dbContext
                    .Cinemas
                    .FirstOrDefaultAsync(c => c.Id == cinemaGuid);

                if (cinema == null)
                {
                    this.ModelState.AddModelError(String.Empty, InvalidCinemaIdMessage);
                    return View(model);
                }

                var cinemaMovie = await dbContext
                    .CinemasMovies
                    .FirstOrDefaultAsync(cm =>
                        cm.MovieId == movieGuid &&
                        cm.CinemaId == cinemaGuid);

                if (viewModel.IsSelected)
                {
                    if (cinemaMovie == null)
                    {
                        entitiesToAdd.Add(new CinemaMovie()
                        {
                            Cinema = cinema,
                            Movie = movie
                        });
                    }
                    else
                    {
                        cinemaMovie.IsDeleted = false;
                    }
                }
                else
                {
                    if (cinemaMovie != null)
                    {
                        cinemaMovie.IsDeleted = true;
                    }
                }
            }

            await dbContext.CinemasMovies.AddRangeAsync(entitiesToAdd);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Cinema");
        }
    }
}
