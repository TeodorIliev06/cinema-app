namespace CinemaApp.Data.Seeding.DTOs
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;

    using static Common.EntityValidationConstants.Movie;

    public class ImportMovieDto : IMapTo<Movie>, IHaveCustomMappings
    {
        [Required]
        [StringLength(IdMaxLength, MinimumLength = IdMinLength)]
        public string Id { get; set; } = null!;

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(GenreMaxLength, MinimumLength = GenreMinLength)]
        public string Genre { get; set; } = null!;

        [Required]
        public string ReleaseDate { get; set; } = null!;

        [Required]
        [StringLength(DirectorNameMaxLength, MinimumLength = DirectorNameMinLength)]
        public string Director { get; set; } = null!;

        [Range(DurationMinValue, DurationMaxValue)]
        public int Duration { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ImportMovieDto, Movie>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => Guid.Parse(s.Id)))
                .ForMember(d => d.ReleaseDate, opt 
                    => opt.Ignore());
        }
    }
}
