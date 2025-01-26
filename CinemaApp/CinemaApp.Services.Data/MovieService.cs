namespace CinemaApp.Services.Data
{
    using System.Globalization;

    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;
    using CinemaApp.Web.ViewModels.Movie;
    using CinemaApp.Web.ViewModels.Cinema;
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Data.Repositories.Contracts;

    using static Common.EntityValidationConstants.Movie;

    public class MovieService(
        IRepository<Movie, Guid> movieRepository,
        IRepository<Cinema, Guid> cinemaRepository,
        IRepository<CinemaMovie, object> cinemaMovieRepository) : BaseService, IMovieService
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

        public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid id)
        {
            var movie = await movieRepository.GetByIdAsync(id);

            var viewModel = new MovieDetailsViewModel();
            if (movie != null)
            {
                AutoMapperConfig.MapperInstance.Map(movie, viewModel);
            }

            return viewModel;
        }

        public async Task<AddMovieToCinemaViewModel?> GetAddMovieToCinemaViewModelByIdAsync(Guid id)
        {
            var movie = await movieRepository
                .GetByIdAsync(id);

            AddMovieToCinemaViewModel? viewModel = null;
            if (movie != null)
            {
                viewModel = new AddMovieToCinemaViewModel()
                {
                    Id = id.ToString(),
                    Title = movie.Title,
                    Cinemas = await cinemaRepository
                        .GetAllAttached()
                        .Include(c => c.CinemaMovies)
                        .ThenInclude(cm => cm.Movie)
                        .Where(c => c.IsDeleted == false)
                        .Select(c => new CinemaCheckBoxItemDto()
                        {
                            Id = c.Id.ToString(),
                            Name = c.Name,
                            Location = c.Location,
                            IsSelected = c.CinemaMovies
                                .Any(cm =>
                                    cm.Movie.Id == id &&
                                    cm.IsDeleted == false)
                        })
                        .ToArrayAsync()
                };
            }

            return viewModel;
        }

        public async Task<bool> AddMovieToCinemasAsync(Guid movieId, AddMovieToCinemaViewModel model)
        {
            var movie = await movieRepository.GetByIdAsync(movieId);

            if (movie == null)
            {
                return false;
            }

            var entitiesToAdd = new List<CinemaMovie>();
            foreach (var viewModel in model.Cinemas)
            {
                var cinemaGuid = Guid.Empty;
                bool isCinemaGuidValid = this.IsGuidValid(viewModel.Id, ref cinemaGuid);

                if (!isCinemaGuidValid)
                {
                    //TODO: Return enum with error messages
                    //this.ModelState.AddModelError(String.Empty, InvalidCinemaIdMessage);
                    //return View(model);

                    return false;
                }

                var cinema = await cinemaRepository.GetByIdAsync(cinemaGuid);

                if (cinema == null)
                {
                    //TODO: Return enum with error messages
                    return false;
                }

                var cinemaMovie = await cinemaMovieRepository
                    .FirstOrDefaultAsync(cm =>
                        cm.MovieId == movieId &&
                        cm.CinemaId == cinemaGuid);

                if (viewModel.IsSelected)
                {
                    if (cinemaMovie == null)
                    {
                        entitiesToAdd.Add(new CinemaMovie()
                        {
                            Cinema = cinema,
                            Movie = movie
                        });
                    }
                    else
                    {
                        cinemaMovie.IsDeleted = false;
                    }
                }
                else
                {
                    if (cinemaMovie != null)
                    {
                        cinemaMovie.IsDeleted = true;
                    }
                }
            }

            await cinemaMovieRepository.AddRangeAsync(entitiesToAdd.ToArray());

            return true;
        }
    }
}
