namespace CinemaApp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;
    using Extensions;

    public class CinemaMovieConfiguration : IEntityTypeConfiguration<CinemaMovie>
    {
        public void Configure(EntityTypeBuilder<CinemaMovie> builder)
        {
            builder
                .HasKey(cm => new { cm.CinemaId, cm.MovieId });

            builder
                .Property(cm => cm.IsDeleted)
                .HasDefaultValue(false);

            builder
                .Property(cm => cm.AvailableTickets)
                .IsRequired()
                .HasDefaultValue(0);

            builder
                .HasOne(cm => cm.Movie)
                .WithMany(m => m.CinemaMovies)
                .HasForeignKey(cm => cm.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(cm => cm.Cinema)
                .WithMany(c => c.CinemaMovies)
                .HasForeignKey(cm => cm.CinemaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .SeedDataFromJson("Datasets/cinemasMovies.json");
        }
    }
}
