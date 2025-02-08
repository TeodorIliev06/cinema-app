﻿namespace CinemaApp.Services.Data.Contracts
{
    using CinemaApp.Web.ViewModels.CinemaMovie;
    using Web.ViewModels.Movie;

    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesViewModel>> GetAllMoviesAsync();

        Task<bool> AddMovieAsync(AddMovieFormModel model);

        Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid movieGuid);

        Task<AddMovieToCinemaViewModel?> GetAddMovieToCinemaViewModelByIdAsync(Guid movieGuid);

        Task<bool> AddMovieToCinemasAsync(Guid movieGuid, AddMovieToCinemaViewModel model);

        Task<EditMovieFormModel?> GetEditMovieFormModelByIdAsync(Guid movieGuid);

        Task<bool> EditMovieAsync(EditMovieFormModel model);

        Task<AvailableTicketsViewModel?> GetAvailableTicketsByIdAsync(Guid cinemaGuid, Guid movieGuid);
    }
}
