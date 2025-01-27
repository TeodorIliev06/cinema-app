using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Data.Contracts
{
    using CinemaApp.Web.ViewModels.Watchlist;

    public interface IWatchlistService
    {
        Task<IEnumerable<ApplicationUserWatchlistViewModel>> GetUserWatchlistByUserIdAsync(string userId);

        Task<bool> AddMovieToUserWatchlistAsync(Guid movieGuid, string userId);
    }
}
