namespace CinemaApp.WebApi
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Web.ViewModels;
    using Services.Mapping;
    using Services.Data.Contracts;
    using Web.Infrastructure.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
            string? cinemaWebAppOrigin = builder.Configuration.GetValue<string>("ClientOrigins:CinemaWebApp");

            // Add services to the container.
            builder.Services
                .AddDbContext<CinemaDbContext>(cfg =>
                {
                    cfg.UseSqlServer(connectionString);
                });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.RegisterRepositories(typeof(ApplicationUser).Assembly);
            builder.Services.RegisterUserDefinedServices(typeof(IMovieService).Assembly);

            builder.Services.AddCors(cfg =>
            {
                cfg.AddPolicy("AllowAll", policyBld =>
                {
                    policyBld
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });

                if (!string.IsNullOrWhiteSpace(cinemaWebAppOrigin))
                {
                    cfg.AddPolicy("AllowMyServer", policyBld =>
                    {
                        policyBld
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithOrigins(cinemaWebAppOrigin);
                    });
                }
            });

            var app = builder.Build();

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            if (!string.IsNullOrWhiteSpace(cinemaWebAppOrigin))
            {
                app.UseCors("AllowMyServer");
            }

            app.MapControllers();

            app.Run();
        }
    }
}
