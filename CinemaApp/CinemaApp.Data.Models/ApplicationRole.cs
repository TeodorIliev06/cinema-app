namespace CinemaApp.Data.Models
{
    using CinemaApp.Data.Models.Contracts;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationRole : IdentityRole<Guid>, ISoftDeletable
    {
        public ApplicationRole()
        {

        }

        public ApplicationRole(string roleName) : this()
        {
            this.Name = roleName;
        }

        public bool IsDeleted { get; set; }
    }
}
