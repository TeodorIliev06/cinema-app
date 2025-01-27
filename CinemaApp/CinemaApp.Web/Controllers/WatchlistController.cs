namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;

    using Data.Models;
    using CinemaApp.Common;
    using CinemaApp.Services.Data.Contracts;

    using static Common.ErrorMessages.Watchlist;

    [Authorize]
    public class WatchlistController(
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

            bool result = await watchlistService.RemoveMovieFromUserWatchlistAsync(movieGuid, userId);
            if (result == false)
            {
                TempData["ErrorMessage"] = RemoveFromWatchlistNotSuccessfulMessage;
                return RedirectToAction("Index", "Movie");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
