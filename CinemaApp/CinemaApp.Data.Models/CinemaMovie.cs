namespace CinemaApp.Data.Models
{
    using CinemaApp.Data.Models.Contracts;

    public class CinemaMovie : ISoftDeletable
    {
        public Guid MovieId { get; set; }

        public virtual Movie Movie { get; set; } = null!;

        public Guid CinemaId { get; set; }

        public virtual Cinema Cinema { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public int AvailableTickets { get; set; }
    }
}
