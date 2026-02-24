using Microsoft.AspNetCore.Identity;
using Webstore.API.Domain;
using Webstore.API.Domain.Entities.Identity;
using Webstore.API.Infrastructure.Extensions;

namespace Webstore.API.Infrastructure.Persistence.Seeding;

sealed internal class IdentitySeeder(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IConfiguration configuration,
    ILogger<IdentitySeeder> logger
) : IDbSeeder<AppIdentityDbContext>
{
    public async Task SeedAsync(AppIdentityDbContext context)
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[]
        {
            new { Name = DomainConst.IdentityRoles.SysAdmin, Description = "Full system access" },
            new { Name = DomainConst.IdentityRoles.Admin, Description = "Limited full system access" },
            new { Name = DomainConst.IdentityRoles.Vendor, Description = "Vendor" },
            new { Name = DomainConst.IdentityRoles.Customer, Description = "Customer" },
        };

        foreach (var roleData in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleData.Name);
            if (!roleExists)
            {
                var role = new ApplicationRole
                {
                    Name = roleData.Name,
                    Description = roleData.Description,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    logger.LogInformation("Role '{RoleName}' created successfully", roleData.Name);
                }
                else
                {
                    logger.LogError("Failed to create role '{RoleName}': {Errors}",
                        roleData.Name,
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("Role '{RoleName}' already exists", roleData.Name);
            }
        }
    }

    private async Task SeedAdminUserAsync()
    {
        const string adminEmail = "nikitinsn6@gmail.com";

        // Ensure Administrator role exists
        var roleExists = await roleManager.RoleExistsAsync(DomainConst.IdentityRoles.SysAdmin);
        if (!roleExists)
        {
            logger.LogError("Administrator role not found after seeding. Something went wrong.");

            return;
        }

        // Check if admin user already exists
        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin != null)
        {
            logger.LogInformation("Admin user already exists: {Email}", adminEmail);

            return;
        }

        // Get admin password from configuration or use default
        var adminPassword = configuration["Auth:AdminPassword"] ?? "esEAdminx_13.wwe123";

        // Create admin user
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "System",
            LastName = "Administrator",
            IsActive = true,
            PhoneNumberConfirmed = true
        };

        var createResult = await userManager.CreateAsync(adminUser, adminPassword);

        if (!createResult.Succeeded)
        {
            logger.LogError("Failed to create admin user: {Errors}",
                string.Join(", ", createResult.Errors.Select(e => e.Description)));

            return;
        }

        // Assign Administrator role
        var roleResult = await userManager.AddToRoleAsync(adminUser, DomainConst.IdentityRoles.SysAdmin);

        if (!roleResult.Succeeded)
        {
            logger.LogError("Failed to assign Administrator role: {Errors}",
                string.Join(", ", roleResult.Errors.Select(e => e.Description)));

            return;
        }

        // Add admin claims
        await userManager.AddClaimsAsync(adminUser, [
            new System.Security.Claims.Claim("FullAccess", "true"),
            new System.Security.Claims.Claim("ManageUsers", "true"),
            new System.Security.Claims.Claim("ManageVendors", "true"),
            new System.Security.Claims.Claim("ManageRoles", "true")
        ]);

        logger.LogInformation("Admin user created successfully: {Email}", adminEmail);
        logger.LogWarning("Default admin password is set. Please change it after first login!");
    }
}