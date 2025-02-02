namespace CinemaApp.Services.Data
{
    using Mapping;
    using Data.Contracts;
    using CinemaApp.Data.Models;
    using Web.ViewModels.CinemaMovie;
    using CinemaApp.Data.Repositories.Contracts;

    public class TicketService(
        IRepository<CinemaMovie, object> cinemaMovieRepository) : ITicketService
    {
        public async Task<bool> SetAvailableTicketsAsync(SetAvailableTicketsViewModel model)
        {
            var cinemaMovie = AutoMapperConfig.MapperInstance.Map<CinemaMovie>(model);

            return await cinemaMovieRepository.UpdateAsync(cinemaMovie);
        }
    }
}
