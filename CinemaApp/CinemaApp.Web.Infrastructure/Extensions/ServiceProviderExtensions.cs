namespace CinemaApp.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using CinemaApp.Common;
    using CinemaApp.Data.Models;
    using System.Data;

    public static class ServiceProviderExtensions
    {
        public static async Task SeedAdminAsync(
            this IServiceProvider serviceProvider,
            string adminEmail,
            string adminUsername,
            string adminPassword)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminRole = ApplicationConstants.AdminRoleName;

            bool isRoleExisting = await roleManager
                .RoleExistsAsync(adminRole);
            if (!isRoleExisting)
            {
                await roleManager.CreateAsync(new ApplicationRole(adminRole));
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser()
                {
                    UserName = adminUsername,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, adminRole);
                }
            }
        }
    }
}
