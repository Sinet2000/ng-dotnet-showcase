using Webstore.API.Infrastructure.Options;

namespace Webstore.API.Extensions;

public static class HostBuilderExtensions
{
    public static IHostApplicationBuilder ConfigureOptions(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOptions<AuthenticationConfigOptions>()
            .BindConfiguration(AuthenticationConfigOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddOptions<CorsConfigOptions>()
            .BindConfiguration(CorsConfigOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return builder;
    }
}