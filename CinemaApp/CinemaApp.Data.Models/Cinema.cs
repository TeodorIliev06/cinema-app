﻿namespace CinemaApp.Data.Models
{
    using CinemaApp.Data.Models.Contracts;

    public class Cinema : ISoftDeletable
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = null!;

        public string Location { get; set; } = null!;

        public virtual ICollection<CinemaMovie> CinemaMovies { get; set; }
            = new HashSet<CinemaMovie>();

        public virtual ICollection<Ticket> Tickets { get; set; }
            = new HashSet<Ticket>();

        public bool IsDeleted { get; set; }
    }
}
