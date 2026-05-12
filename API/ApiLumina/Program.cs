
using ApiLumina.Area.Identity.Configuration;
using ApiLumina.Area.Identity.Mapping;
using ApiLumina.Area.Identity.Services;
using ApiLumina.Area.Identity.Services.Interfaces;
using ApiLumina.Hubs;
using Lumina.Data;
using Lumina.Data.Models;
using Lumina.Data.Repository;
using Lumina.Data.Repository.Interfaces;
using Lumina.Services.Core;
using Lumina.Services.Core.Interfaces;
using Lumina.Services.Core.Mapping;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nest;
using System.ComponentModel;
using System.Text;

namespace ApiLumina;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentException("Connection string is not correct.");


        builder.Services.AddDbContext<CinemaAppDBContext>(options =>
        {
            options.UseSqlServer(connectionString)
            ;
        });




        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddAutoMapper(
        typeof(Program).Assembly,
        typeof(MovieProfile).Assembly,
        typeof(CinemaProfile).Assembly,
        typeof(AuthProfile).Assembly,
        typeof(ScreeningProfile).Assembly);

        builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
        .AddEntityFrameworkStores<CinemaAppDBContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddScoped<IMovieRepository, MovieRepository>();
        builder.Services.AddScoped<ICinemaRepository, CinemaRepository>();
        builder.Services.AddScoped<ITicketRepository, TicketRepository>();
        builder.Services.AddScoped<IScreeningRepository, ScreeningRepository>();
        builder.Services.AddScoped<IWatchlistRepository, WatchlistRepository>();


        builder.Services.AddScoped<IMovieService, MovieService>();
        builder.Services.AddScoped<ICinemaService, CinemaService>();
        builder.Services.AddScoped<ITicketService, TicketService>();
        builder.Services.AddScoped<IScreeningService, ScreeningService>();
        builder.Services.AddScoped<IWatchlistService, WatchlistService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IPermissionsService, PermissionsService>();
        builder.Services.AddScoped<IManageUserService, ManageUserService>();
        builder.Services.AddScoped<IMessageService, MessageService>();

        builder.Services.AddSingleton<PresenceTracker>();

        builder.Services.Configure<JwtSettings>(
            builder.Configuration.GetSection("JwtSettings"));
        var jwtSecret = builder.Configuration["JwtSettings:Secret"];
        if (string.IsNullOrWhiteSpace(jwtSecret) || jwtSecret == "__SET_IN_USER_SECRETS__")
        {
            throw new InvalidOperationException("JWT secret is missing. Set 'JwtSettings:Secret' via dotnet user-secrets or environment variables.");
        }  

        builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/presenceHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cinema API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Write token in this format: Bearer {your token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
        });

        
        builder.Services.AddAuthorization(options => options.AddPolicy("MovieCreatePolicy", policy => policy.RequireClaim("Permission", "Permissions.Movie.Create")));
        builder.Services.AddAuthorization(options => options.AddPolicy("MovieEditPolicy", policy => policy.RequireClaim("Permission", "Permissions.Movie.Edit")));
        builder.Services.AddAuthorization(options => options.AddPolicy("MovieDeletePolicy", policy => policy.RequireClaim("Permission", "Permissions.Movie.Delete")));
        builder.Services.AddAuthorization(options => options.AddPolicy("UserViewPolicy", policy => policy.RequireClaim("Permission", "Permissions.Users.View")));
        builder.Services.AddAuthorization(options => options.AddPolicy("UserDeletePolicy", policy => policy.RequireClaim("Permission", "Permissions.Users.Delete")));
        builder.Services.AddAuthorization(options => options.AddPolicy("CreateUserPolicy", policy => policy.RequireClaim("Permission", "Permissions.Users.Create")));
        builder.Services.AddAuthorization(options => options.AddPolicy("UserEditPolicy", policy => policy.RequireClaim("Permission", "Permissions.Users.Edit")));
        builder.Services.AddAuthorization(options => options.AddPolicy("EditCinemaPolicy", policy => policy.RequireClaim("Permission", "Permissions.Cinema.Edit")));
        builder.Services.AddAuthorization(options => options.AddPolicy("CreateCinemaPolicy", policy => policy.RequireClaim("Permission", "Permissions.Cinema.Create")));
        builder.Services.AddAuthorization(options => options.AddPolicy("CinemaDeletePolicy", policy => policy.RequireClaim("Permission", "Permissions.Cinema.Delete")));
        builder.Services.AddAuthorization(options => options.AddPolicy("ScreeningViewPolicy", policy => policy.RequireClaim("Permission", "Permissions.Screenings.View")));
        builder.Services.AddAuthorization(options => options.AddPolicy("ScreeningCreatePolicy", policy => policy.RequireClaim("Permission", "Permissions.Screenings.Create")));
        builder.Services.AddAuthorization(options => options.AddPolicy("ScreeningDeletePolicy", policy => policy.RequireClaim("Permission", "Permissions.Screenings.Delete")));
        builder.Services.AddAuthorization(options => options.AddPolicy("ScreeningEditPolicy", policy => policy.RequireClaim("Permission", "Permissions.Screenings.Edit")));
        

        builder.Services.AddSignalR();
        builder.Services.AddHttpContextAccessor();


        builder.Services.AddCors(options => options.AddPolicy("SignalRPolicy", policy => policy

        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()));

        builder.Services.AddMassTransit(x => 
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        });

        var app = builder.Build();

        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();


        app.UseRouting();



        app.UseCors("_myAllowSpecificOrigins");
        app.UseCors("SignalRPolicy");


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<NotificationHub>("/notificationHub");
        app.MapHub<SupportChatHub>("/supportChatHub");
        app.MapHub<PresenceHub>("/presenceHub");

        app.MapControllers();


        app.Run();
    }
}
