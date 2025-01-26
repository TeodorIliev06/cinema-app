namespace CinemaApp.Services.Data
{
    using CinemaApp.Data.Models;
    using CinemaApp.Web.ViewModels.Movie;
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Data.Repositories.Contracts;
    using CinemaApp.Services.Mapping;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;

    using static Common.EntityValidationConstants.Movie;

    public class MovieService(
        IRepository<Movie, Guid> movieRepository) : IMovieService
    {
        public async Task<IEnumerable<AllMoviesViewModel>> GetAllMoviesAsync()
        {
            return await movieRepository
                .GetAllAttached()
                .To<AllMoviesViewModel>()
                .ToArrayAsync();
        }

        public async Task<bool> AddMovieAsync(AddMovieFormModel model)
        {
            bool isReleaseDateValid = DateTime
                .TryParseExact(model.ReleaseDate, ReleaseDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);

            if (!isReleaseDateValid)
            {
                return false;
            }

            var movie = new Movie();
            AutoMapperConfig.MapperInstance.Map(model, movie);
            movie.ReleaseDate = releaseDate;

            await movieRepository.AddAsync(movie);

            return true;
        }
    }
}
