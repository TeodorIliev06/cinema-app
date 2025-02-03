namespace CinemaApp.WebApi
{
    using CinemaApp.Data;
    using CinemaApp.Data.Models;
    using CinemaApp.Services.Data.Contracts;
    using CinemaApp.Services.Mapping;
    using CinemaApp.Web.Infrastructure.Extensions;
    using CinemaApp.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

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

            app.UseCors("AllowAll");

            app.MapControllers();

            app.Run();
        }
    }
}
