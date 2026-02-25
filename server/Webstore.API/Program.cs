using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Webstore.API.Extensions;
using Webstore.API.Handlers;
using Webstore.API.Infrastructure;
using Webstore.API.Infrastructure.Options;
using Webstore.API.Infrastructure.Persistence;
using Webstore.API.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOptions();
builder.AddInfrastructure();
builder.AddServiceDefaults();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;

    // Strict JSON for numeric types (prevents integer|string schemas in OpenAPI and avoids silently accepting "123" for numbers)
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;

    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    var corsOptions = builder.Configuration.GetSection(CorsConfigOptions.SectionName).Get<CorsConfigOptions>();
    if (corsOptions is null)
    {
        throw new Exception("Cors options not found");
    }

    options.AddPolicy(corsOptions.PolicyName, policy =>
    {
        if (corsOptions.AllowedOrigins.Length > 0)
        {
            policy.WithOrigins(corsOptions.AllowedOrigins);
        }
        else
        {
            policy.AllowAnyOrigin();
        }

        policy.AllowAnyMethod().AllowAnyHeader();

        if (corsOptions is { AllowCredentials: true, AllowedOrigins.Length: > 0 })
        {
            policy.AllowCredentials();
        }
    });
});

builder.AddOpenApiDocument();

var app = builder.Build();

var corsOptions = builder.Configuration.GetSection(CorsConfigOptions.SectionName).Get<CorsConfigOptions>()!;

using (var scope = app.Services.CreateScope())
{
    // Migrate DBs
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();

    var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
    await identityDbContext.Database.MigrateAsync();

    // You can also run seeding there
}

app.MapDefaultEndpoints();
app.UseStatusCodePages();

app.UseCors(corsOptions.PolicyName);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();

app.Run();
