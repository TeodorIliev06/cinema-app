namespace CinemaApp.Web
{
    using CinemaApp.Data.Seeding;
    using CinemaApp.Data.Seeding.DTOs;
    using CinemaApp.Services.Data;
    using CinemaApp.Services.Data.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Web.ViewModels;
    using Services.Mapping;
    using Infrastructure.Extensions;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
            string adminEmail = builder.Configuration.GetValue<string>("Administrator:Email")!;
            string adminUsername = builder.Configuration.GetValue<string>("Administrator:Username")!;
            string adminPassword = builder.Configuration.GetValue<string>("Administrator:Password")!;
            string movieJsonPath = Path.Combine(AppContext.BaseDirectory,
                builder.Configuration.GetValue<string>("Seed:MoviesJson")!);
            string cinemaJsonPath = Path.Combine(AppContext.BaseDirectory,
                builder.Configuration.GetValue<string>("Seed:CinemasJson")!);

            // Add services to the container.
            builder.Services.AddDbContext<CinemaDbContext>(cfg =>
                cfg.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(cfg =>
                {
                    IdentityOptionsConfigurator.Configure(builder, cfg);
                })
                .AddEntityFrameworkStores<CinemaDbContext>()
                .AddRoles<ApplicationRole>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddUserManager<UserManager<ApplicationUser>>();
            //.AddUserStore<ApplicationUser>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
            });

            builder.Services.RegisterRepositories(typeof(ApplicationUser).Assembly);
            builder.Services.RegisterUserDefinedServices(typeof(IMovieService).Assembly);

            var app = builder.Build();

            AutoMapperConfig
                .RegisterMappings(typeof(ErrorViewModel).Assembly,
                    typeof(ImportMovieDto).Assembly);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            await app.SeedAdminAsync(adminEmail, adminUsername, adminPassword);

            await app.SeedDataAsync(
                new SeederConfiguration()
                {
                    MethodName = "SeedMoviesAsync",
                    JsonPath = movieJsonPath
                },
                new SeederConfiguration()
                {
                    MethodName = "SeedCinemasAsync",
                    JsonPath = cinemaJsonPath
                }
            );

            app.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.ApplyMigrations();
            app.Run();
        }
    }
}
