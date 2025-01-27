namespace CinemaApp.Web.ViewModels.Watchlist
{
    using AutoMapper;
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;
    using static Common.EntityValidationConstants.Movie;

    public class ApplicationUserWatchlistViewModel : IMapFrom<ApplicationUserMovie>, IHaveCustomMappings
    {
        public string MovieId { get; set; } = null!;

        public string MovieTitle { get; set; } = null!;

        public string MovieGenre { get; set; } = null!;

        public string ReleaseDate { get; set; } = null!;

        public string? MovieImageUrl { get; set; }
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUserMovie, ApplicationUserWatchlistViewModel>()
                .ForMember(d => d.MovieId,
                    x =>
                        x.MapFrom(s => s.MovieId.ToString()))
                .ForMember(d => d.ReleaseDate,
                    x =>
                        x.MapFrom(s => s.Movie.ReleaseDate.ToString(ReleaseDateFormat)));
        }
    }
}
