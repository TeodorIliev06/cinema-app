﻿namespace CinemaApp.Web.Controllers
{
    using System.Globalization;

    using Microsoft.AspNetCore.Mvc;

    using Data;
    using Data.Models;
    using ViewModels.Movie;

    using static Common.EntityValidationConstants.Movie;

    public class MovieController(CinemaDbContext dbContext) : Controller
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
        public IActionResult Details(string id)
        {
            bool isGuidValid = Guid.TryParse(id, out Guid guid);

            if (!isGuidValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var movie = dbContext
                .Movies
                .FirstOrDefault(m => m.Id == guid);

            if (movie == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }
    }
}
