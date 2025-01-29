﻿namespace CinemaApp.Services.Data
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;
    using CinemaApp.Web.ViewModels.Movie;
    using CinemaApp.Web.ViewModels.Cinema;
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Data.Repositories.Contracts;

    public class CinemaService(
        IRepository<Cinema, Guid> cinemaRepository) : ICinemaService
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

        public async Task<CinemaDetailsViewModel?> GetCinemaDetailsByIdAsync(Guid cinemaGuid)
        {
            var cinema = await cinemaRepository
                .GetAllAttached()
                .Include(c => c.CinemaMovies)
                .ThenInclude(cm => cm.Movie)
                .FirstOrDefaultAsync(c => c.Id == cinemaGuid);

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

        public async Task<EditCinemaFormModel?> GetCinemaForEditByIdAsync(Guid id)
        {
            var viewModel = await cinemaRepository
                .GetAllAttached()
                .To<EditCinemaFormModel>()
                .FirstOrDefaultAsync(c => 
                    c.Id.ToLower() == id.ToString().ToLower());

            return viewModel;
        }

        public async Task<bool> EditCinemaAsync(EditCinemaFormModel model)
        {
            var cinema = AutoMapperConfig.MapperInstance.Map<EditCinemaFormModel, Cinema>(model);

            bool result = await cinemaRepository.UpdateAsync(cinema);

            return result;
        }
    }
}
