namespace CinemaApp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    using static Common.EntityValidationConstants.Cinema;
    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);

            builder
                .Property(c => c.Location)
                .IsRequired()
                .HasMaxLength(LocationMaxLength);

            builder
                .HasData(this.SeedCinemas());
        }

        private IEnumerable<Cinema> SeedCinemas()
        {
            var cinemas = new List<Cinema>()
            {
                new Cinema
                {
                    Name = "Starlight Cinema",
                    Location = "123 Main Street, Springfield"
                },
                new Cinema
                {
                    Name = "Galaxy Theaters",
                    Location = "456 Elm Street, Metropolis"
                }
            };

            return cinemas;
        }
    }
}
