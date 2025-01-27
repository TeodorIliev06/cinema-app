namespace CinemaApp.Web.Controllers
{
    using CinemaApp.Common;
    using CinemaApp.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Authorization;

    using Data;
    using Data.Models;
    using ViewModels.Watchlist;

    using static Common.EntityValidationConstants.Movie;
    using static Common.ErrorMessages.Watchlist;

    [Authorize]
    public class WatchlistController(
        CinemaDbContext dbContext,
        IWatchlistService watchlistService,
        UserManager<ApplicationUser> userManager) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userId = userManager.GetUserId(User)!;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToPage("/Identity/Account/Login");
            }

            var model =
                await watchlistService.GetUserWatchlistByUserIdAsync(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWatchlist(string? movieId)
        {
            var movieGuid = Guid.Empty;

            bool isGuidValid = ValidationUtils.IsGuidValid(movieId, ref movieGuid);
            if (!isGuidValid)
            {
                return RedirectToAction("Index", "Movie");
            }

            string userId = userManager.GetUserId(User)!;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToPage("/Identity/Account/Login");
            }

            bool result = await watchlistService.AddMovieToUserWatchlistAsync(movieGuid, userId);
            if (result == false)
            {
                TempData["ErrorMessage"] = AddToWatchlistNotSuccessfulMessage;
                return RedirectToAction("Index", "Movie");
            }

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
