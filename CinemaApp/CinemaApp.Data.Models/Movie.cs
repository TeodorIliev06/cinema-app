namespace CinemaApp.Data.Models
{
    using CinemaApp.Data.Models.Contracts;

    public class Movie: ISoftDeletable
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        public string Director { get; set; } = null!;

        public int Duration { get; set; }

        public string Description { get; set; } = null!;
        public string? ImageUrl { get; set; }

        public virtual ICollection<CinemaMovie> CinemaMovies { get; set; } 
            = new HashSet<CinemaMovie>();

        public virtual ICollection<ApplicationUserMovie> ApplicationUserMovies { get; set; }
            = new HashSet<ApplicationUserMovie>();

        public virtual ICollection<Ticket> Tickets { get; set; }
            = new HashSet<Ticket>();

        public bool IsDeleted { get; set; }
    }
}
