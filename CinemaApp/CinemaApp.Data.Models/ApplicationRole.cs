namespace CinemaApp.Data.Models
{
    using CinemaApp.Data.Models.Contracts;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationRole : IdentityRole<Guid>, ISoftDeletable
    {
        public ApplicationRole() 
            : base()
        {

        }

        public bool IsDeleted { get; set; }
    }
}
