using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

namespace Webstore.API.ServiceDefaults;

public static class OpenApiExtensions
{
    public static IApplicationBuilder MapOpenApiEndpoints(this WebApplication app)
    {
        var openApiSection = app.Configuration.GetSection("OpenApi");

        if (!openApiSection.Exists())
        {
            return app;
        }

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference("/api-docs", opt =>
            {
                opt.WithTitle("Webstore API")
                .WithTheme(ScalarTheme.Mars)
                .PreserveSchemaPropertyOrder()
                .HideSearch();
            });
            app.MapGet("/api-docs", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();
        }

        return app;
    }

    public static IHostApplicationBuilder AddOpenApiDocument(this IHostApplicationBuilder builder)
    {
        var openApiSection = builder.Configuration.GetSection("OpenApi");
        if (!openApiSection.Exists())
        {
            return builder;
        }

        var title = openApiSection["Document:Title"] ?? "API";
        var description = openApiSection["Document:Description"] ?? string.Empty;
        var version = openApiSection["Document:Version"] ?? "v1";

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = title,
                    Description = description,
                    Version = version
                };
                return Task.CompletedTask;
            });
        });

        return builder;
    }
}