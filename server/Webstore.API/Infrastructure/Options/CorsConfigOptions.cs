namespace Webstore.API.Infrastructure.Options;

public sealed class CorsConfigOptions
{
    public const string SectionName = "Cors";

    public string PolicyName { get; set; } = "AllowedOrigins";
    public string[] AllowedOrigins { get; set; } = [];
    public bool AllowCredentials { get; set; }
}
