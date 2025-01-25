using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Services.Data.Contracts
{
    using Web.ViewModels.Cinema;

    public interface ICinemaService
    {
        Task<IEnumerable<CinemaIndexViewModel>> GetAllOrderedByLocationAsync();
        Task AddCinemaAsync(AddCinemaFormModel model);
        Task<CinemaDetailsViewModel?> GetCinemaDetailsByIdAsync(Guid id);
    }
}
