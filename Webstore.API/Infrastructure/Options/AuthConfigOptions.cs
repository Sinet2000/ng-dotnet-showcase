namespace Webstore.API.Infrastructure.Options;

public sealed class AuthenticationConfigOptions
{
    public const string SectionName = "Auth";

    public string ClientBaseUri { get; init; } = "http://localhost:4200";

    public JwtOptions Jwt { get; init; } = new();

    public sealed class JwtOptions
    {
        public string Issuer { get; init; } = string.Empty;

        public string Audience { get; init; } = string.Empty;

        public string SecretKey { get; init; } = string.Empty;

        public int ExpiryMinutes { get; init; } = 60;
    }
}
