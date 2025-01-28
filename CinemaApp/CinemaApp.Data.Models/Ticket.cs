namespace CinemaApp.Data.Models
{
    using CinemaApp.Data.Models.Contracts;

    public class Ticket : ISoftDeletable
    {
        public Guid Id { get; set; }

        public decimal Price { get; set; }

        public Guid CinemaId { get; set; }

        public virtual Cinema Cinema { get; set; } = null!;

        public Guid MovieId { get; set; }

        public virtual Movie Movie { get; set; } = null!;

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
