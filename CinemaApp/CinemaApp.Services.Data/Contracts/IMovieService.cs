namespace CinemaApp.Services.Data.Contracts
{
    using Web.ViewModels.Movie;

    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesViewModel>> GetAllMoviesAsync();

        Task<bool> AddMovieAsync(AddMovieFormModel model);

        Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid movieGuid);

        Task<AddMovieToCinemaViewModel?> GetAddMovieToCinemaViewModelByIdAsync(Guid movieGuid);

        Task<bool> AddMovieToCinemasAsync(Guid movieGuid, AddMovieToCinemaViewModel model);
    }
}
