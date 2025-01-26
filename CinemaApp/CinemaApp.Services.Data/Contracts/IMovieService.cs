namespace CinemaApp.Services.Data.Contracts
{
    using Web.ViewModels.Movie;

    public interface IMovieService
    {
        Task<IEnumerable<AllMoviesViewModel>> GetAllMoviesAsync();

        Task<bool> AddMovieAsync(AddMovieFormModel model);

        Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid id);

        Task<AddMovieToCinemaViewModel?> GetAddMovieToCinemaViewModelByIdAsync(Guid id);

        Task<bool> AddMovieToCinemasAsync(Guid movieId, AddMovieToCinemaViewModel model);
    }
}
