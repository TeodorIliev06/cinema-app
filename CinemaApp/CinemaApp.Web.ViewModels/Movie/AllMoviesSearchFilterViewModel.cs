using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Web.ViewModels.Movie
{
    public class AllMoviesSearchFilterViewModel
    {
        public IEnumerable<AllMoviesViewModel>? Movies { get; set; }

        public string? SearchQuery { get; set; }

        public string? GenreFilter { get; set; }

        public IEnumerable<string>? AllGenres { get; set; }

        public string? YearFilter { get; set; }
    }
}
