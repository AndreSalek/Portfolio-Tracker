using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortfolioTracker.Core;
using PortfolioTracker.Core.Data;
using PortfolioTracker.Core.Data.Seed;
using PortfolioTracker.Core.Infrastructure;
using PortfolioTracker.Core.Models;
using PortfolioTracker.Core.Services;
using PortfolioTracker.Web.ViewModels;

namespace PortfolioTracker.Web
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
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
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
            services.AddScoped<EmailService>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseExceptionHandler("/Home/Error");
            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();


            // AI temporary?? maybe
            using (var scope = app.Services.CreateScope())
            {
                await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);
            }


            app.Run();
        }
    }
}
