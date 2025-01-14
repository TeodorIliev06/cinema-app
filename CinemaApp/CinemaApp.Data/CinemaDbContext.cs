namespace CinemaApp.Data
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class CinemaDbContext : DbContext
    {
        public CinemaDbContext()
        {

        }

        public CinemaDbContext(DbContextOptions options)
        : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; } = null!;
    }
}
