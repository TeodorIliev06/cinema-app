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
        ITicketService ticketService,
        IMovieService movieService) : BaseApiController(managerService)
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
            var isCinemaGuidValid = ValidationUtils.TryGetGuid(id, out Guid cinemaGuid);
            if (!isCinemaGuidValid)
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
        [ProducesResponseType(typeof(AvailableTicketsViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAvailableTickets([FromBody] GetAvailableTicketsFormModel model)
        {
            var isCinemaGuidValid = ValidationUtils.TryGetGuid(model.CinemaId, out Guid cinemaGuid);
            if (!isCinemaGuidValid)
            {
                return BadRequest();
            }

            var isMovieGuidValid = ValidationUtils.TryGetGuid(model.MovieId, out Guid movieGuid);
            if (!isMovieGuidValid)
            {
                return BadRequest();
            }

            var availableTicketsViewModel = await movieService
                .GetAvailableTicketsByIdAsync(cinemaGuid, movieGuid);
            if (availableTicketsViewModel == null)
            {
                return BadRequest();
            }

            return Ok(availableTicketsViewModel);
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
