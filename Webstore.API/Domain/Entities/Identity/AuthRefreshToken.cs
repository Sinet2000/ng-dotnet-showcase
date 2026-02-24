namespace Webstore.API.Domain.Entities.Identity;

public class AuthRefreshToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    // Navigation properties
    public virtual ApplicationUser User { get; set; } = null!;

    // Computed properties
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;

    public AuthRefreshToken()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public AuthRefreshToken(
        int userId,
        string tokenHash,
        string salt,
        DateTime expiresAt,
        string? ipAddress = null,
        string? userAgent = null) : this()
    {
        UserId = userId;
        TokenHash = tokenHash;
        Salt = salt;
        ExpiresAt = expiresAt;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    public void Revoke()
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
    }
}