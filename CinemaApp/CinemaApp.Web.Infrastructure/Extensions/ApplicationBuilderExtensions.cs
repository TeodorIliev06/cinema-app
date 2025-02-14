namespace CinemaApp.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Data;
    using CinemaApp.Data.Seeding;
    using static Common.ErrorMessages.Seeding;

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

        public static async Task<IApplicationBuilder> SeedDataAsync(
            this IApplicationBuilder app,
            params SeederConfiguration[] configurations)
        {
            await using var scope = app.ApplicationServices.CreateAsyncScope();
            var serviceProvider = scope.ServiceProvider;

            Type seederType = typeof(DbSeeder);
            foreach (var cfg in configurations)
            {
                try
                {
                    var methodInfo = seederType.GetMethod(cfg.MethodName);

                    if (methodInfo == null)
                    {
                        throw new InvalidOperationException(string.Format(InvalidMethodName, cfg.MethodName));
                    }

                    await (Task)methodInfo.Invoke(null, new object[]
                    {
                        serviceProvider,
                        cfg.JsonPath
                    })!;
                }
                catch (Exception e)
                {
                    Console.WriteLine(ExecutionError, cfg.MethodName, e.Message);
                    throw;
                }
            }

            return app;
        }
    }
}
