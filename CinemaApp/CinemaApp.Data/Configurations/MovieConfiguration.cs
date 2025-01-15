namespace CinemaApp.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Models;

    using static Common.EntityValidationConstants.Movie;

    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(TitleMaxLength);

            builder
                .Property(m => m.Genre)
                .IsRequired()
                .HasMaxLength(GenreMaxLength);

            builder
                .Property(m => m.Director)
                .IsRequired()
                .HasMaxLength(DirectorNameMaxLength);

            builder
                .Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);

            builder
                .HasData(this.SeedMovies());
        }

        private List<Movie> SeedMovies()
        {
            var movies = new List<Movie>()
            {
                new Movie
                {
                    Title = "Inception",
                    Genre = "Sci-Fi",
                    ReleaseDate = new DateTime(2010, 7, 16),
                    Director = "Christopher Nolan",
                    Duration = 148,
                    Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a CEO."
                },
                new Movie
                {
                    Title = "The Dark Knight",
                    Genre = "Action",
                    ReleaseDate = new DateTime(2008, 7, 18),
                    Director = "Christopher Nolan",
                    Duration = 152,
                    Description = "When the menace known as The Joker emerges from his mysterious past, he wreaks havoc and chaos on the people of Gotham."
                }
            };

            return movies;
        }
    }
}
