namespace CinemaApp.Web.ViewModels.Movie
{
    using System.ComponentModel.DataAnnotations;

    using static Common.EntityValidationConstants.Movie;

    public class AddMovieFormModel
    {
        [MaxLength(TitleMaxLength)]
        public required string Title { get; set; } = null!;

        [MinLength(GenreMinLength)]
        [MaxLength(GenreMaxLength)]
        public required string Genre { get; set; } = null!;

        public required string ReleaseDate { get; set; } = null!;

        [Range(DurationMinValue, DurationMaxValue)]
        public int Duration { get; set; }

        [MinLength(DirectorNameMinLength)]
        [MaxLength(DirectorNameMaxLength)]
        public required string Director { get; set; } = null!;

        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public required string Description { get; set; } = null!;
    }
}
