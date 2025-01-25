namespace CinemaApp.Web
{
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
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

            // Add services to the container.
            builder.Services.AddDbContext<CinemaDbContext>(cfg =>
                cfg.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(cfg =>
                {
                    IdentityOptionsConfigurator.Configure(builder, cfg);
                })
                .AddEntityFrameworkStores<CinemaDbContext>()
                .AddRoles<IdentityRole<Guid>>()
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

            builder.Services.AddScoped<ICinemaService, CinemaService>();

            var app = builder.Build();

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.ApplyMigrations();
            app.Run();
        }
    }
}
