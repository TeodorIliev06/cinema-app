namespace CinemaApp.Services.Data
{
    using System.Globalization;
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Common;
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;
    using CinemaApp.Web.ViewModels.Movie;
    using CinemaApp.Web.ViewModels.Cinema;
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Data.Repositories.Contracts;
    using CinemaApp.Web.ViewModels.CinemaMovie;
    using static Common.EntityValidationConstants.Movie;
    using static Common.ApplicationConstants;

    public class MovieService(
        IRepository<Movie, Guid> movieRepository,
        IRepository<Cinema, Guid> cinemaRepository,
        IRepository<CinemaMovie, object> cinemaMovieRepository) : IMovieService
    {
        public async Task<IEnumerable<AllMoviesViewModel>> GetAllMoviesAsync()
        {
            return await movieRepository
                .GetAllAttached()
                .Where(c => c.IsDeleted == false)
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

        public async Task<MovieDetailsViewModel?> GetMovieDetailsByIdAsync(Guid movieGuid)
        {
            var movie = await movieRepository.GetByIdAsync(movieGuid);

            var viewModel = new MovieDetailsViewModel();
            if (movie != null)
            {
                AutoMapperConfig.MapperInstance.Map(movie, viewModel);
            }

            return viewModel;
        }

        public async Task<AddMovieToCinemaViewModel?> GetAddMovieToCinemaViewModelByIdAsync(Guid movieGuid)
        {
            var movie = await movieRepository
                .GetByIdAsync(movieGuid);

            AddMovieToCinemaViewModel? viewModel = null;
            if (movie != null)
            {
                viewModel = new AddMovieToCinemaViewModel()
                {
                    Id = movieGuid.ToString(),
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
                                    cm.Movie.Id == movieGuid &&
                                    cm.IsDeleted == false)
                        })
                        .ToArrayAsync()
                };
            }

            return viewModel;
        }

        public async Task<bool> AddMovieToCinemasAsync(Guid movieGuid, AddMovieToCinemaViewModel model)
        {
            var movie = await movieRepository.GetByIdAsync(movieGuid);

            if (movie == null)
            {
                return false;
            }

            var entitiesToAdd = new List<CinemaMovie>();
            foreach (var viewModel in model.Cinemas)
            {
                var cinemaGuid = Guid.Empty;
                bool isCinemaGuidValid = ValidationUtils.IsGuidValid(viewModel.Id, ref cinemaGuid);

                if (!isCinemaGuidValid)
                {
                    //TODO: Return enum with error messages
                    //this.ModelState.AddModelError(String.Empty, InvalidCinemaIdMessage);
                    //return View(model);

                    return false;
                }

                var cinema = await cinemaRepository.GetByIdAsync(cinemaGuid);

                if (cinema == null || cinema.IsDeleted)
                {
                    //TODO: Return enum with error messages
                    return false;
                }

                var cinemaMovie = await cinemaMovieRepository
                    .FirstOrDefaultAsync(cm =>
                        cm.MovieId == movieGuid &&
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

        public async Task<EditMovieFormModel?> GetEditMovieFormModelByIdAsync(Guid movieGuid)
        {
            var viewModel = await movieRepository
                .GetAllAttached()
                .Where(m => m.IsDeleted == false)
                .To<EditMovieFormModel>()
                .FirstOrDefaultAsync(m =>
                    m.Id.ToLower() == movieGuid.ToString().ToLower());

            if (viewModel != null &&
                viewModel.ImageUrl.Equals(ApplicationConstants.NoImageUrl))
            {
                viewModel.ImageUrl = "No image";
            }

            return viewModel;
        }

        public async Task<bool> EditMovieAsync(EditMovieFormModel model)
        {
            var movieGuid = Guid.Empty;
            bool isMovieGuidValid = ValidationUtils.IsGuidValid(model.Id, ref movieGuid);

            if (!isMovieGuidValid)
            {
                return false;
            }

            var editedMovie = AutoMapperConfig.MapperInstance.Map<Movie>(model);
            editedMovie.Id = movieGuid;

            bool isReleaseDateValid = DateTime
                .TryParseExact(model.ReleaseDate, ReleaseDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);
            if (!isReleaseDateValid)
            {
                return false;
            }

            editedMovie.ReleaseDate = releaseDate;
            if (model.ImageUrl == null ||
                model.ImageUrl.Equals("No image"))
            {
                editedMovie.ImageUrl = NoImageUrl;
            }

            return await movieRepository.UpdateAsync(editedMovie);
        }

        public async Task<AvailableTicketsViewModel?> GetAvailableTicketsByIdAsync(Guid cinemaGuid, Guid movieGuid)
        {
            var cinemaMovie = await cinemaMovieRepository
                .FirstOrDefaultAsync(cm =>
                    cm.MovieId == movieGuid &&
                    cm.CinemaId == cinemaGuid);

            AvailableTicketsViewModel viewModel = null;
            if (cinemaMovie != null)
            {
                viewModel = new AvailableTicketsViewModel()
                {
                    CinemaId = cinemaGuid.ToString(),
                    MovieId = movieGuid.ToString(),
                    AvailableTickets = cinemaMovie.AvailableTickets,
                    Quantity = 0
                };
            }

            return viewModel;
        }

        public async Task<DeleteMovieViewModel?> GetMovieForDeleteByIdAsync(Guid movieGuid)
        {
            var cinema = await movieRepository
                .GetAllAttached()
                .Where(m => m.IsDeleted == false)
                .To<DeleteMovieViewModel>()
                .FirstOrDefaultAsync(c =>
                    c.Id.ToLower() == movieGuid.ToString().ToLower());

            return cinema;
        }

        public async Task<bool> SoftDeleteMovieAsync(Guid movieGuid)
        {
            var movie = await movieRepository
                .FirstOrDefaultAsync(c =>
                    c.Id.ToString().ToLower() == movieGuid.ToString().ToLower());
            if (movie == null)
            {
                return false;
            }

            movie.IsDeleted = true;

            return await movieRepository.UpdateAsync(movie);
        }
    }
}
