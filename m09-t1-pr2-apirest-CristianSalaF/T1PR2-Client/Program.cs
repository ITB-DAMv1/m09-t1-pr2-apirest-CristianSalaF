using Microsoft.AspNetCore.Authentication.Cookies;

namespace T1PR2_Client
{
    /// <summary>
    /// Main class for the ASP.NET Core application.
    /// This class contains the entry point for the application.
    /// It is responsible for configuring and running the web application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method for the ASP.NET Core application.
        /// This method is the entry point of the application.
        /// It sets up the web application, configures services, and runs the application.
        /// It includes setting up the configuration, services, middleware, and routing.
        /// It also includes setting up authentication and session management.
        /// This method is responsible for starting the application and handling incoming requests.
        /// It is the main entry point for the application and is called when the application starts.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            await Task.Delay(3000);

            // Accés a la configuració del fitxer appsettings.json
            string apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("API base URL not found");

            //Afegim Servei de connexió HttpClient
            //ApiFilms es el nom que li donem a la connexio de l'Api
            builder.Services.AddHttpClient("ApiGameJam", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";
                    options.AccessDeniedPath = "/AccessDenied";
                });
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            //Activem les Sessions abans de l'enroutament
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
