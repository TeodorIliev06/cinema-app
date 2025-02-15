namespace CinemaApp.Web.Infrastructure.Extensions
{
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Data;
    using Common;
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
            var dbContext = serviceProvider.GetRequiredService<CinemaDbContext>();

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
                        dbContext,
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

        public static async Task<IApplicationBuilder> SeedAllDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var seedingPaths = GetSeedingPaths(configuration);

            if (!seedingPaths.Any())
            {
                throw new InvalidOperationException(NoSeedingPaths);
            }

            var configurations = seedingPaths
                .Select(kvp => new SeederConfiguration()
                {
                    MethodName = kvp.Key,
                    JsonPath = kvp.Value
                }).ToArray();

            await app.SeedDataAsync(configurations);
            return app;
        }

        private static Dictionary<string, string> GetSeedingPaths(IConfiguration configuration)
        {
            var seedSection = configuration.GetSection(ApplicationConstants.DefaultSeedConfigurationSection);
            var seedingPaths = new Dictionary<string, string>();

            var seederMethods = typeof(DbSeeder)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name.StartsWith("Seed") && m.Name.EndsWith("Async"))
                .Select(m => m.Name)
                .ToList();

            foreach (var methodName in seederMethods)
            {
                // Convert method name to configuration key
                var cfgKey = methodName
                                 .Replace("Seed", "")
                                 .Replace("Async", "")
                                 + "Json";

                var jsonPath = seedSection[cfgKey];
                if (!string.IsNullOrEmpty(jsonPath))
                {
                    seedingPaths[methodName] = Path.Combine(AppContext.BaseDirectory, jsonPath);
                }
            }

            return seedingPaths;
        }
    }
}
