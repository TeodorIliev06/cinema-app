namespace CinemaApp.Services.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CinemaApp.Data.Models;
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Web.ViewModels.Admin.UserManagement;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<AllUsersViewModel>> GetAllUsersAsync()
        {
            var allUsers = await this.userManager.Users.ToArrayAsync();
            var allUsersViewModel = new List<AllUsersViewModel>();

            foreach (var user in allUsers)
            {
                IEnumerable<string> roles = await this.userManager.GetRolesAsync(user);

                allUsersViewModel.Add(new AllUsersViewModel()
                {
                    Id = user.Id.ToString(),
                    Email = user.Email,
                    Roles = roles
                });
            }

            return allUsersViewModel;
        }

        public async Task<bool> UserExistsByIdAsync(Guid userId)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());

            return user != null;
        }

        public async Task<bool> AssignUserToRoleAsync(Guid userId, string role)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            bool isRoleExisting = await roleManager.RoleExistsAsync(role);

            if (user == null || !isRoleExisting)
            {
                return false;
            }

            bool isInRole = await this.userManager.IsInRoleAsync(user, role);
            if (!isInRole)
            {
                var result = await this.userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> RemoveUserRoleAsync(Guid userId, string role)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());
            bool isRoleExisting = await roleManager.RoleExistsAsync(role);

            if (user == null || !isRoleExisting)
            {
                return false;
            }

            bool isInRole = await this.userManager.IsInRoleAsync(user, role);
            if (isInRole)
            {
                var result = await this.userManager.RemoveFromRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await this.userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return false;
            }

            var result = await this.userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }
    }
}
