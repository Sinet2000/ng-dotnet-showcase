namespace Webstore.API.Application.Identity.Models;

public record AuthResult(
    bool Succeeded,
    string? Token,
    string? RefreshToken,
    DateTimeOffset? ExpiresAt,
    int? UserId,
    string? Email,
    string? FirstName,
    string? LastName,
    List<string>? Roles,
    string? Error,
    Dictionary<string, string[]>? ValidationErrors = null)
{
    public static AuthResult Success(
        string accessToken,
        string refreshToken,
        DateTimeOffset expiresAt,
        int userId,
        string email,
        string firstName,
        string lastName,
        List<string> roles)
        => new(true, accessToken, refreshToken, expiresAt, userId, email, firstName, lastName, roles, null);

    public static AuthResult Failure(string error)
        => new(false, null, null, null, null, null, null, null, null, error);

    public static AuthResult ValidationFailure(Dictionary<string, string[]> validationErrors)
        => new(false, null, null, null, null, null, null, null, null, "Validation failed", validationErrors);
}