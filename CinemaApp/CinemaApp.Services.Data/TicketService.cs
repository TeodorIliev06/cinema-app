namespace CinemaApp.Services.Data
{
    using Mapping;
    using Data.Contracts;
    using CinemaApp.Data.Models;
    using Web.ViewModels.CinemaMovie;
    using CinemaApp.Data.Repositories.Contracts;
    using CinemaApp.Web.ViewModels.Ticket;
    using Microsoft.AspNetCore.Identity;

    public class TicketService(
        IRepository<Ticket, Guid> ticketRepository,
        IRepository<CinemaMovie, object> cinemaMovieRepository) : ITicketService
    {
        public async Task<bool> SetAvailableTicketsAsync(SetAvailableTicketsViewModel model)
        {
            var cinemaMovie = AutoMapperConfig.MapperInstance.Map<CinemaMovie>(model);

            return await cinemaMovieRepository.UpdateAsync(cinemaMovie);
        }

        public async Task<bool> BuyTicketsAsync(
            Guid movieGuid, Guid cinemaGuid,
            AvailableTicketsViewModel model, string userId)
        {
            var cinemaMovie = await cinemaMovieRepository.FirstOrDefaultAsync(cm => 
                    cm.MovieId == movieGuid && cm.CinemaId == cinemaGuid);

            if (cinemaMovie == null)
            {
                return false;
            }

            if (cinemaMovie.AvailableTickets < model.Quantity)
            {
                return false; // Not enough tickets available
            }

            var random = new Random();
            var ticketsToBuy = new List<Ticket>();
            for (int i = 0; i < model.Quantity; i++)
            {
                ticketsToBuy.Add(new Ticket
                {
                    Id = Guid.NewGuid(),
                    Price = (decimal)random.Next(5, 20),
                    CinemaId = cinemaGuid,
                    MovieId = movieGuid,
                    UserId = Guid.Parse(userId),
                    IsDeleted = false
                });
            }

            cinemaMovie.AvailableTickets -= model.Quantity;

            // Save tickets
            await ticketRepository.AddRangeAsync(ticketsToBuy.ToArray());

            return true;
        }
    }
}
