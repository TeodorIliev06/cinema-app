namespace CinemaApp.Web.ViewModels.Movie
{
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;

    public class CinemaMovieViewModel : IMapFrom<Movie>
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public int Duration { get; set; }

        public string Description { get; set; } = null!;
    }
}
