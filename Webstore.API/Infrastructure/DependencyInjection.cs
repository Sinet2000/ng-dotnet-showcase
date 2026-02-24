using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Webstore.API.Application.Identity.Abstractions;
using Webstore.API.Domain.Entities.Identity;
using Webstore.API.Infrastructure.Options;
using Webstore.API.Infrastructure.Persistence;

namespace Webstore.API.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        // Database contexts
        builder.AddNpgsqlDbContext<AppIdentityDbContext>("webstore-identity");
        builder.AddNpgsqlDbContext<AppDbContext>("webstore-main");

        // Caching
        builder.Services.AddMemoryCache();

        // Identity
        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

        // Authentication
        var authConfig = builder.Configuration.GetSection(AuthenticationConfigOptions.SectionName)
            .Get<AuthenticationConfigOptions>();

        ArgumentNullException.ThrowIfNull(authConfig, nameof(authConfig));

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = authConfig.Jwt.Issuer,
                    ValidAudience = authConfig.Jwt.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(authConfig.Jwt.SecretKey)
                    ),

                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name
                };
            });

        builder.Services.AddAuthorization();

        // Application services
        builder.Services.AddScoped<IAuthService, Application.Identity.AuthService>();

        // Seeders
        // builder.Services.AddScoped<IDbSeeder<AppDbContext>, ClassifierSeeder>();
        // builder.Services.AddScoped<IDbSeeder<AppIdentityDbContext>, IdentitySeeder>();

        // builder.Services.SeedInBackgroundAsync<AppDbContext>();
        // builder.Services.SeedInBackgroundAsync<AppIdentityDbContext>();

        return builder;
    }
}
