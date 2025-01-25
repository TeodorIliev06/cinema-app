namespace CinemaApp.Services.Data
{
    using Mapping;
    using Contracts;
    using CinemaApp.Data.Models;
    using Web.ViewModels.Cinema;
    using CinemaApp.Data.Repositories.Contracts;

    using Microsoft.EntityFrameworkCore;
    using CinemaApp.Web.ViewModels.Movie;
    using System;

    public class CinemaService(IRepository<Cinema, Guid> cinemaRepository) : ICinemaService
    {
        public async Task<IEnumerable<CinemaIndexViewModel>> GetAllOrderedByLocationAsync()
        {
            IEnumerable<CinemaIndexViewModel> cinemas = await cinemaRepository
                .GetAllAttached()
                .OrderBy(c => c.Location)
                .To<CinemaIndexViewModel>()
                .ToListAsync();

            return cinemas;
        }

        public async Task AddCinemaAsync(AddCinemaFormModel model)
        {
            var cinema = new Cinema();
            AutoMapperConfig.MapperInstance.Map(model, cinema);

            await cinemaRepository.AddAsync(cinema);
        }

        public async Task<CinemaDetailsViewModel?> GetCinemaDetailsByIdAsync(Guid id)
        {
            var cinema = await cinemaRepository
                .GetAllAttached()
                .Include(c => c.CinemaMovies)
                .ThenInclude(cm => cm.Movie)
                .FirstOrDefaultAsync(c => c.Id == id);

            CinemaDetailsViewModel? viewModel = null;
            if (cinema != null)
            {
                viewModel = new CinemaDetailsViewModel()
                {
                    Name = cinema.Name,
                    Location = cinema.Location,
                    Movies = cinema.CinemaMovies
                        .Where(cm => cm.IsDeleted == false)
                        .Select(cm => new CinemaMovieViewModel()
                        {
                            Title = cm.Movie.Title,
                            Duration = cm.Movie.Duration,
                        })
                        .ToList()
                };
            }

            return viewModel;
        }
    }
}
