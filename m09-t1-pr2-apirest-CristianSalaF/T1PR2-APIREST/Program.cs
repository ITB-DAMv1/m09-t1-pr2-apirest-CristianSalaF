
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text;
using T1PR2_APIREST.Context;
using T1PR2_APIREST.Data;
using T1PR2_APIREST.Hubs;
using T1PR2_APIREST.Models;

namespace T1PR2_APIREST
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DevelopmentConnection");
    object value = builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

    builder.Services.AddIdentity<User, IdentityRole>(options =>
     {
         options.Password.RequireDigit = true;
         options.Password.RequiredLength = 6;
         options.Password.RequireNonAlphanumeric = true;
         options.Password.RequireUppercase = true;
         options.Password.RequireLowercase = true;

         options.User.RequireUniqueEmail = true;

         options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
         options.Lockout.MaxFailedAccessAttempts = 5;
         options.Lockout.AllowedForNewUsers = true;

         // Configuració del login
         options.SignIn.RequireConfirmedEmail = false;
     })
         .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    // Token setup and validation
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],

                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])) 
            };
        });

    builder.Services.AddAuthorization();

    // prevent infinite loop
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins("https://localhost:7147"); // client-side connection
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
        });
    });

    builder.Services.AddOpenApi();

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(opt =>
    {
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });
    });

    builder.Services.AddSignalR();

    //********
    var app = builder.Build();
    //*******
    
    //***** Middlewares ******//

    //App pipeline

    // Crear rols inicials: Admin i User
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            // Create initial roles
            await Tools.RoleTools.CrearRolsInicials(services);
                    
            // Seed the database with initial games
            var context = services.GetRequiredService<AppDbContext>();
            await DbInitializer.Initialize(context, services);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.UseCors();
    app.MapHub<XatHub>("/Xat");

    app.Run();
}
    }
}
