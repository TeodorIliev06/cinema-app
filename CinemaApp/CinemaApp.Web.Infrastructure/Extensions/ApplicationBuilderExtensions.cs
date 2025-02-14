namespace CinemaApp.Web.Infrastructure.Extensions
{
    using CinemaApp.Data.Seeding;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Data;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateAsyncScope();

            var dbContext = serviceScope
                .ServiceProvider
                .GetRequiredService<CinemaDbContext>()!;
            dbContext.Database.Migrate();

            return app;
        }

        public static async Task<IApplicationBuilder> SeedAdminAsync(
            this IApplicationBuilder app,
            string adminEmail,
            string adminUsername,
            string adminPassword)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            await serviceProvider.SeedAdminAsync(adminEmail, adminUsername, adminPassword);

            return app;
        }

        public static async Task<IApplicationBuilder> SeedMoviesAsync(this IApplicationBuilder app, string jsonPath)
        {
            await using var scope = app.ApplicationServices.CreateAsyncScope();
            var serviceProvider = scope.ServiceProvider;

            await DbSeeder.SeedMoviesAsync(serviceProvider, jsonPath);

            return app;
        }
    }
}
