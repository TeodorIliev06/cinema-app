using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Models
{
    using CinemaApp.Data.Models.Contracts;

    public class Manager : ISoftDeletable
    {
        public Guid Id { get; set; }

        public string WorkPhoneNumber { get; set; } = null!;

        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
