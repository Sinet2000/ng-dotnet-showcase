using Microsoft.AspNetCore.Identity;

namespace Webstore.API.Domain.Entities.Identity;

public class ApplicationUserToken : IdentityUserToken<int>
{
    public virtual ApplicationUser User { get; set; } = null!;
}