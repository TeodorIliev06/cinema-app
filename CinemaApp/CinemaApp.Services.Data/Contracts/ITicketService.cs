namespace CinemaApp.Services.Data.Contracts
{
    using CinemaApp.Web.ViewModels.CinemaMovie;
    using CinemaApp.Web.ViewModels.Ticket;

    public interface ITicketService
    {
        public Task<bool> SetAvailableTicketsAsync(SetAvailableTicketsViewModel model);

        public Task<bool> BuyTicketsAsync(
            Guid movieGuid, Guid cinemaGuid,
            AvailableTicketsViewModel model, string userId);
    }
}
