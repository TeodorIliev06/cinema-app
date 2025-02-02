namespace CinemaApp.Web.ViewModels.CinemaMovie
{
    using System.ComponentModel.DataAnnotations;

    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;

    using static Common.ErrorMessages.CinemaMovie;
    using static Common.EntityValidationConstants.CinemaMovie;

    public class SetAvailableTicketsViewModel : IMapTo<CinemaMovie>
    {
        [Required]
        public string CinemaId { get; set; } = null!;

        [Required]
        public string MovieId { get; set; } = null!;

        [Required(ErrorMessage = AvailableTicketsRequiredMessage)]
        [Range(AvailableTicketsMinValue, AvailableTicketsMaxValue, ErrorMessage = AvailableTicketsRangeMessage)]
        public int AvailableTickets { get; set; }
    }
}
