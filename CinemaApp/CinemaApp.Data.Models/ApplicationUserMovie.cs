namespace CinemaApp.Data.Models
{
    using CinemaApp.Data.Models.Contracts;

    public class ApplicationUserMovie : ISoftDeletable
    {
        public Guid ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public Guid MovieId { get; set; }

        public virtual Movie Movie { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
