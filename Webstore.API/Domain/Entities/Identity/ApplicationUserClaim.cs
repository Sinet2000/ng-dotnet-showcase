using Microsoft.AspNetCore.Identity;

namespace Webstore.API.Domain.Entities.Identity;

public class ApplicationUserClaim : IdentityUserClaim<int>
{
    public virtual ApplicationUser User { get; set; } = null!;
}