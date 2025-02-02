namespace CinemaApp.Services.Data.Contracts
{
    using CinemaApp.Web.ViewModels.CinemaMovie;

    public interface ITicketService
    {
        public Task<bool> SetAvailableTicketsAsync(SetAvailableTicketsViewModel model);
    }
}
