namespace CinemaApp.Data.Seeding.DTOs
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Mapping;

    using static Common.EntityValidationConstants.Cinema;

    public class ImportCinemaDto : IMapTo<Movie>, IHaveCustomMappings
    {
        [Required]
        [StringLength(IdMaxLength, MinimumLength = IdMinLength)]
        public string Id { get; set; } = null!;

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(LocationMaxLength, MinimumLength = LocationMinLength)]
        public string Location { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ImportCinemaDto, Cinema>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => Guid.Parse(s.Id)));
        }
    }
}
