namespace CinemaApp.Web.Controllers
{
    using System.Globalization;
    using CinemaApp.Web.ViewModels.Cinema;
    using Microsoft.AspNetCore.Mvc;

    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using ViewModels.Movie;

    using static Common.EntityValidationConstants.Movie;
    using static Common.EntityValidationMessages.Cinema;

    public class MovieController(CinemaDbContext dbContext) : BaseController
    {

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Movie> movies = dbContext
                .Movies
                .ToList();

            return View(movies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AddMovieFormModel model)
        {
            bool isReleaseDateValid = DateTime
                .TryParseExact(model.ReleaseDate, ReleaseDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);

            if (!isReleaseDateValid)
            {
                this.ModelState.AddModelError(nameof(model.ReleaseDate), $"The release date must be in the following format: {ReleaseDateFormat}");
            }

            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var movie = new Movie()
            {
                Title = model.Title,
                Genre = model.Genre,
                ReleaseDate = releaseDate,
                Director = model.Director,
                Duration = model.Duration,
                Description = model.Description
            };

            dbContext.Movies.Add(movie);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(string? id)
        {
            var movieGuid = Guid.Empty;

            bool isGuidValid = this.IsGuidValid(id, ref movieGuid);

            if (!isGuidValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var movie = dbContext
                .Movies
                .FirstOrDefault(m => m.Id == movieGuid);

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

            //Create the view model
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
