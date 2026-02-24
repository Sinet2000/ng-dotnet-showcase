using Microsoft.AspNetCore.Identity;

namespace Webstore.API.Domain.Entities.Identity;

public class ApplicationRoleClaim : IdentityRoleClaim<int>
{
    public virtual ApplicationRole Role { get; set; } = null!;
}