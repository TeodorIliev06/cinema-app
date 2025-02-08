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
            var movieGuid = Guid.Empty;
            bool isMovieGuidValid = ValidationUtils.IsGuidValid(model.MovieId, ref movieGuid);

            var cinemaGuid = Guid.Empty;
            bool isCinemaGuidValid = ValidationUtils.IsGuidValid(model.CinemaId, ref cinemaGuid);

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
