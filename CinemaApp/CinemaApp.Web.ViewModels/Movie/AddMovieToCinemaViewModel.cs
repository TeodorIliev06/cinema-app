namespace CinemaApp.Web.ViewModels.Movie
{
    using System.ComponentModel.DataAnnotations;

    using Cinema;

    using static CinemaApp.Common.EntityValidationConstants.Movie;
    public class AddMovieToCinemaViewModel
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        public IList<CinemaCheckBoxItemDto> Cinemas { get; set; }
            = new List<CinemaCheckBoxItemDto>();
    }
}
