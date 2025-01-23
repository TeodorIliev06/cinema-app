namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Authorization;

    using Data;
    using Data.Models;
    using ViewModels.Watchlist;

    using static Common.EntityValidationConstants.Movie;

    [Authorize]
    public class WatchlistController(
        CinemaDbContext dbContext,
        UserManager<ApplicationUser> userManager) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = userManager.GetUserId(User)!;

            var model = await dbContext
                .UsersMovies
                .Include(um => um.Movie)
                .Where(um =>
                    um.ApplicationUserId.ToString().ToLower() ==
                    userId.ToLower() &&
                    um.IsDeleted == false)
                .Select(um => new ApplicationUserWatchlistViewModel()
                {
                    MovieId = um.MovieId.ToString(),
                    Title = um.Movie.Title,
                    Genre = um.Movie.Genre,
                    ReleaseDate = um.Movie.ReleaseDate.ToString(ReleaseDateFormat),
                    ImageUrl = um.Movie.ImageUrl
                })
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWatchlist(string? movieId)
        {
            var movieGuid = Guid.Empty;

            bool isGuidValid = this.IsGuidValid(movieId, ref movieGuid);

            if (!isGuidValid)
            {
                return RedirectToAction("Index", "Movie");
            }

            var movie = await dbContext
                .Movies
                .FirstOrDefaultAsync(m => m.Id == movieGuid);

            if (movie == null)
            {
                return RedirectToAction("Index", "Movie");
            }

            var userGuid = Guid.Parse(userManager.GetUserId(this.User)!);

            //Seed applicationUserMovies in json.
            var applicationUserMovie = await dbContext
                .UsersMovies
                .FirstOrDefaultAsync(um =>
                    um.MovieId == movieGuid &&
                    um.ApplicationUserId == userGuid);

            if (applicationUserMovie == null)
            {
                var newUserMovie = new ApplicationUserMovie()
                {
                    ApplicationUserId = userGuid,
                    MovieId = movieGuid,
                    IsDeleted = false
                };

                await dbContext.UsersMovies.AddAsync(newUserMovie);
            }
            else if (applicationUserMovie.IsDeleted)
            {
                applicationUserMovie.IsDeleted = false;
            }

            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromWatchlist(string? movieId)
        {
            var movieGuid = Guid.Empty;

            bool isGuidValid = this.IsGuidValid(movieId, ref movieGuid);

            if (!isGuidValid)
            {
                return RedirectToAction("Index", "Movie");
            }

            var movie = await dbContext
                .Movies
                .FirstOrDefaultAsync(m => m.Id == movieGuid);

            if (movie == null)
            {
                return RedirectToAction("Index", "Movie");
            }

            var userGuid = Guid.Parse(userManager.GetUserId(this.User)!);

            var applicationUserMovie = await dbContext
                .UsersMovies
                .Where(um => um.IsDeleted == false)
                .FirstOrDefaultAsync(um =>
                    um.MovieId == movieGuid &&
                    um.ApplicationUserId == userGuid);

            if (applicationUserMovie != null)
            {
                applicationUserMovie.IsDeleted = true;
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
