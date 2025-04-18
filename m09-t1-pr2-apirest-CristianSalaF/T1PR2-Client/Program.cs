using Microsoft.AspNetCore.Authentication.Cookies;

namespace T1PR2_Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            await Task.Delay(3000);


            //TODO: added methods but still partial in the Login.cshtml.cs and Login.cshtml page, lacking a Logout, and i've yet to check the API connection.
            //TODO: make the api connect with the host like i've connected the chat to "just work" without authentication.
            //
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
