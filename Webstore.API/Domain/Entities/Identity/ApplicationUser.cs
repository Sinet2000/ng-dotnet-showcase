using Microsoft.AspNetCore.Identity;

namespace Webstore.API.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsBlocked { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = [];
    public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = [];
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; } = [];
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; } = [];
    public virtual ICollection<AuthRefreshToken> RefreshTokens { get; set; } = [];

    public string FullName => $"{FirstName} {LastName}".Trim();
}
