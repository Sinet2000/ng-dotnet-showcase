using Microsoft.EntityFrameworkCore;
using Webstore.API.Domain.Entities;
using Webstore.API.Infrastructure.Persistence.Conversions;

namespace Webstore.API.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Catalog> Catalogs => Set<Catalog>();
    public DbSet<CatalogItem> CatalogItems => Set<CatalogItem>();
    public DbSet<Product> Products => Set<Product>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<DateTime>()
            .HaveConversion<UtcDateTimeConverter>();

        configurationBuilder.Properties<decimal>()
            .HavePrecision(18, 4); // For prices, etc.
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        // UpdateTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private static void UpdateTimestamps()
    {
        //var entries = ChangeTracker.Entries()
        //    .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        //foreach (var entry in entries)
        //{
        //    var entity = (BaseEntity)entry.Entity;

        //    if (entry.State == EntityState.Modified)
        //    {
        //        entity.UpdatedAt = DateTime.UtcNow;
        //    }
        //}
    }
}
