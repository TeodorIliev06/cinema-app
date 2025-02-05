namespace CinemaApp.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using CinemaApp.Common;
    using CinemaApp.Web.ViewModels.Cinema;
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Web.Infrastructure.Attributes;
    using CinemaApp.Web.ViewModels.CinemaMovie;

    public class TicketApiController(
        IManagerService managerService,
        ICinemaService cinemaService,
        ITicketService ticketService) : BaseApiController(managerService)
    {
        [HttpGet("[action]/{id?}")]
        [ManagerOnly]
        [ProducesResponseType(typeof(CinemaDetailsViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMoviesByCinema(string? id)
        {
            var cinemaGuid = Guid.Empty;
            var isIdValid = ValidationUtils.IsGuidValid(id, ref cinemaGuid);
            if (!isIdValid)
            {
                return BadRequest();
            }

            var viewModel = await cinemaService
                .GetCinemaDetailsByIdAsync(cinemaGuid);
            if (viewModel == null)
            {
                return NotFound();
            }

            return Ok(viewModel);
        }

        [HttpPost("[action]")]
        [ManagerOnly]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAvailableTickets([FromBody] SetAvailableTicketsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isSuccess = await ticketService.SetAvailableTicketsAsync(model);
            if (!isSuccess)
            {
                return BadRequest();
            }

            return Ok("Ticket availability updated successfully");
        }
    }
}
