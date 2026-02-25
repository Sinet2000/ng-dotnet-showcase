using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Webstore.API.Infrastructure.Extensions;

internal static class DbSeederExtensions
{
    private const string ActivitySourceName = "DbSeeding";
    private static readonly ActivitySource ActivitySource = new(ActivitySourceName);

    /// <summary>
    /// Enables automatic seeding on application startup. Call once after registering all seeders.
    /// </summary>
    public static IServiceCollection SeedInBackgroundAsync<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddOpenTelemetry().WithTracing(tracing => tracing.AddSource(ActivitySourceName));
        services.AddHostedService(sp => new SeederHostedService<TContext>(sp));

        return services;
    }

    /// <summary>
    /// Manually seed the database (runs all registered seeders in order)
    /// </summary>
    public static async Task SeedAsync<TContext>(this IServiceProvider services)
        where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var scopeServices = scope.ServiceProvider;
        var logger = scopeServices.GetRequiredService<ILogger<TContext>>();
        var context = scopeServices.GetRequiredService<TContext>();
        var seeders = scopeServices.GetServices<IDbSeeder<TContext>>().ToList();

        if (!seeders.Any())
        {
            logger.LogWarning("No seeders registered for context {DbContextName}", typeof(TContext).Name);

            return;
        }

        using var activity = ActivitySource.StartActivity($"Seeding operation {typeof(TContext).Name}");

        try
        {
            logger.LogInformation("Starting database seeding for context {DbContextName} with {SeederCount} seeders",
                typeof(TContext).Name, seeders.Count);

            var strategy = context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                foreach (var seeder in seeders)
                {
                    var seederName = seeder.GetType().Name;

                    using var seedActivity = ActivitySource.StartActivity($"Executing {seederName}");

                    try
                    {
                        logger.LogInformation("Running seeder: {SeederName}", seederName);
                        await seeder.SeedAsync(context);
                        logger.LogInformation("Successfully completed seeder: {SeederName}", seederName);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error in seeder {SeederName}", seederName);
                        seedActivity?.SetExceptionTags(ex);

                        throw;
                    }
                }
            });

            logger.LogInformation("Successfully seeded database for context {DbContextName}", typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database for context {DbContextName}", typeof(TContext).Name);
            activity?.SetExceptionTags(ex);

            throw;
        }
    }

    private class SeederHostedService<TContext>(IServiceProvider serviceProvider)
        : BackgroundService where TContext : DbContext
    {
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await serviceProvider.SeedAsync<TContext>();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}

public interface IDbSeeder<in TContext> where TContext : DbContext
{
    Task SeedAsync(TContext context);
}
