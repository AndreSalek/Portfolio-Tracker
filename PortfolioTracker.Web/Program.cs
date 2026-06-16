using BackendLibrary;
using Frontend.Common;
using Frontend.Data;
using Frontend.Data.Models;
using Frontend.Data.Seed;
using Frontend.Services;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Core.Configuration;
using PortfolioTracker.Core.Interfaces;
using PortfolioTracker.Core.Models;
using PortfolioTracker.Core.Services;

namespace Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            string connectionString = builder.Configuration["AppSecrets:ConnectionString"] ??
                throw new InvalidOperationException("Connection string not found.");

            Dictionary<Platform, string[]> platformKeyMap = new Dictionary<Platform, string[]>()
            {
                { Platform.Kraken, new []{ nameof(PlatformKey.Secret), nameof(PlatformKey.Public)} }
            };
            builder.Logging.AddConsole();
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            // TODO: Change RequireConfirmedAccount after full register and login implementation
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Lockout.MaxFailedAccessAttempts = 5;  
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); 
                options.Lockout.AllowedForNewUsers = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            

        services.AddSingleton<Dictionary<Platform, string[]>>(platformKeyMap);
            services.AddScoped<KrakenHttpHandler>();
            services.AddValidation();

            // Registering named HttpClients, these have transient lifetimes
            services.AddHttpClient<KrakenService>(httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://api.kraken.com");
            })
            .AddHttpMessageHandler<KrakenHttpHandler>();

            services.Configure<EmailSettings>(
            builder.Configuration.GetSection("Email"));
            builder.Services.AddScoped<EmailService>();

            services.AddScoped<IUserService, UserService>();


            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


            // AI temporary?? maybe
            using (var scope = app.Services.CreateScope())
            {
                await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);
            }


            app.Run();
        }
    }
}
