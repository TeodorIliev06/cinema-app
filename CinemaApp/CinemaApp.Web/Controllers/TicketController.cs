namespace CinemaApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using CinemaApp.Common;
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Web.ViewModels.CinemaMovie;
    using CinemaApp.Services.Data;
    using Microsoft.AspNetCore.Identity;
    using CinemaApp.Web.ViewModels.Ticket;

    public class TicketController(
        ITicketService ticketService,
        UserManager<ApplicationUser> userManager,
        IManagerService managerService) : BaseController(managerService)
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BuyTickets(AvailableTicketsViewModel model)
        {
            var isMovieGuidValid = ValidationUtils.TryGetGuid(model.MovieId, out Guid movieGuid);
            var isCinemaGuidValid = ValidationUtils.TryGetGuid(model.CinemaId, out Guid cinemaGuid);

            if (!isMovieGuidValid || !isCinemaGuidValid)

            {
                return RedirectToAction("Details", "Cinema");
            }

            string userId = userManager.GetUserId(User)!;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToPage("/Identity/Account/Login");
            }

            bool isPurchaseSuccessful = await ticketService
                .BuyTicketsAsync(movieGuid, cinemaGuid, model, userId);
            if (!isPurchaseSuccessful)
            {
                //Recreate with TempData error message
                ModelState.AddModelError(string.Empty, "Ticket purchase failed. Please try again.");
                return RedirectToAction("Details", "Cinema");
            }

            return RedirectToAction("Index", "Cinema");
        }
    }
}
