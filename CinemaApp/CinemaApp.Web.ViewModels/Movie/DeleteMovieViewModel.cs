namespace CinemaApp.Web.ViewModels.Movie
{
    using AutoMapper;
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;

    using static Common.EntityValidationConstants.Movie;

    public class DeleteMovieViewModel : IMapFrom<Movie>
    {
        public DeleteMovieViewModel()
        {
            //this.ReleaseDate = DateTime.UtcNow.ToString(ReleaseDateFormat);
        }

        public string Id { get; set; } = null!;

        public string? Title { get; set; }

        public string? Genre { get; set; }

        //public string? ReleaseDate { get; set; }

        public string? ImageUrl { get; set; }
        //public void CreateMappings(IProfileExpression configuration)
        //{
        //    configuration.CreateMap<DeleteMovieViewModel, Movie>()
        //        .ForMember(d => d.ReleaseDate,
        //            x => x.Ignore());
        //}
    }
}
