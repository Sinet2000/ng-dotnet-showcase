
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Webstore.API.Domain.Entities.Identity;
using Webstore.API.Infrastructure.Persistence.Conversions;

namespace Webstore.API.Infrastructure.Persistence;

public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : IdentityDbContext<
    ApplicationUser,
    ApplicationRole,
    int,
    ApplicationUserClaim,
    ApplicationUserRole,
    ApplicationUserLogin,
    ApplicationRoleClaim,
    ApplicationUserToken>(options)
{
    public DbSet<AuthRefreshToken> RefreshTokens => Set<AuthRefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Customize table names
        builder.Entity<ApplicationUser>(b =>
        {
            b.ToTable("users");
            b.Property(u => u.FirstName).HasMaxLength(128);
            b.Property(u => u.LastName).HasMaxLength(128);
        });

        builder.Entity<ApplicationRole>(b =>
        {
            b.ToTable("roles");
            b.Property(r => r.Description).HasMaxLength(512);
        });

        builder.Entity<ApplicationUserRole>(b =>
        {
            b.ToTable("user_roles");
            b.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            b.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        });

        builder.Entity<ApplicationUserClaim>(b =>
        {
            b.ToTable("user_claims");
            b.HasOne(uc => uc.User)
                .WithMany(u => u.Claims)
                .HasForeignKey(uc => uc.UserId);
        });

        builder.Entity<ApplicationUserLogin>(b =>
        {
            b.ToTable("user_logins");
            b.HasOne(ul => ul.User)
                .WithMany(u => u.Logins)
                .HasForeignKey(ul => ul.UserId);
        });

        builder.Entity<ApplicationUserToken>(b =>
        {
            b.ToTable("user_tokens");
            b.HasOne(ut => ut.User)
                .WithMany(u => u.Tokens)
                .HasForeignKey(ut => ut.UserId);
        });

        builder.Entity<ApplicationRoleClaim>(b =>
        {
            b.ToTable("role_claims");
            b.HasOne(rc => rc.Role)
                .WithMany(r => r.RoleClaims)
                .HasForeignKey(rc => rc.RoleId);
        });

        builder.Entity<AuthRefreshToken>(b =>
        {
            b.ToTable("auth_refresh_tokens");
            b.HasKey(rt => rt.Id);

            b.Property(rt => rt.TokenHash)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(rt => rt.Salt)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(rt => rt.IpAddress)
                .HasMaxLength(45); // IPv6 max length

            b.Property(rt => rt.UserAgent)
                .HasMaxLength(512);

            b.HasIndex(rt => rt.TokenHash)
                .IsUnique();

            b.HasIndex(rt => new { rt.UserId, rt.IsRevoked, rt.ExpiresAt });

            b.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<DateTime>()
            .HaveConversion<UtcDateTimeConverter>();

        configurationBuilder.Properties<decimal>()
            .HavePrecision(18, 4); // For prices, etc.
    }
}
