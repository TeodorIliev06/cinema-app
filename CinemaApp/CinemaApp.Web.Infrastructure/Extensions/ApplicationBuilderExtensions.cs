﻿namespace CinemaApp.Web.Infrastructure.Extensions
{
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
    }
}
