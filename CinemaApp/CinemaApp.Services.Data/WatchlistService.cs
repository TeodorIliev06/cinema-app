namespace CinemaApp.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Web.ViewModels.Watchlist;
    using CinemaApp.Data.Repositories.Contracts;

    public class WatchlistService(
        IRepository<ApplicationUserMovie, object> userMovieRepository,
        IRepository<Movie, Guid> movieRepository) : IWatchlistService
    {
        public async Task<IEnumerable<ApplicationUserWatchlistViewModel>> GetUserWatchlistByUserIdAsync(string userId)
        {
            var model = await userMovieRepository
                .GetAllAttached()
                .Include(um => um.Movie)
                .Where(um =>
                    um.ApplicationUserId.ToString().ToLower() ==
                    userId.ToLower() &&
                    um.IsDeleted == false)
                .To<ApplicationUserWatchlistViewModel>()
                .ToListAsync();

            return model;
        }

        public async Task<bool> AddMovieToUserWatchlistAsync(Guid movieGuid, string userId)
        {
            var userGuid = Guid.Parse(userId);
            var movie = await movieRepository
                .GetByIdAsync(movieGuid);

            if (movie == null)
            {
                return false;
            }

            var applicationUserMovie = await userMovieRepository
                .FirstOrDefaultAsync(um =>
                    um.MovieId == movieGuid &&
                    um.ApplicationUserId == userGuid);

            if (applicationUserMovie != null)
            {
                // If the relationship exists and is active, return false (duplicate)
                if (!applicationUserMovie.IsDeleted)
                {
                    return false;
                }

                applicationUserMovie.IsDeleted = false;
                var updateSuccess = await userMovieRepository.UpdateAsync(applicationUserMovie);

                return updateSuccess;
            }

            var newUserMovie = new ApplicationUserMovie
            {
                ApplicationUserId = userGuid,
                MovieId = movieGuid,
                IsDeleted = false
            };

            await userMovieRepository.AddAsync(newUserMovie);
            return true;
        }

        public async Task<bool> RemoveMovieFromUserWatchlistAsync(Guid movieGuid, string userId)
        {
            var movie = await movieRepository
                .GetByIdAsync(movieGuid);

            if (movie == null)
            {
                return false;
            }

            var userGuid = Guid.Parse(userId);

            var applicationUserMovie = await userMovieRepository
                .GetAllAttached()
                .Where(um => um.IsDeleted == false)
                .FirstOrDefaultAsync(um =>
                    um.MovieId == movieGuid &&
                    um.ApplicationUserId == userGuid);

            if (applicationUserMovie != null)
            {
                applicationUserMovie.IsDeleted = true;
                var updateSuccess = await userMovieRepository.UpdateAsync(applicationUserMovie);

                return updateSuccess;
            }

            return true;
        }
    }
}
