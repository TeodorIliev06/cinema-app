namespace CinemaApp.Services.Data.Contracts
{
    using CinemaApp.Web.ViewModels.Watchlist;

    public interface IWatchlistService
    {
        Task<IEnumerable<ApplicationUserWatchlistViewModel>> GetUserWatchlistByUserIdAsync(string userId);

        Task<bool> AddMovieToUserWatchlistAsync(Guid movieGuid, string userId);
        Task<bool> RemoveMovieFromUserWatchlistAsync(Guid movieGuid, string userId);
    }
}
