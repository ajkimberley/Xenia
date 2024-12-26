using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Xenia.WebApi.Swagger;

/// <summary>
/// Represents the <see cref="SwaggerGenOptions"/> setup.
/// </summary>
internal sealed class SwaggerGenOptionsSetup : IConfigureOptions<SwaggerGenOptions>
{
    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "0.0.1",
            Title = "Xenia API",
            Description = "This swagger document describes the available API endpoints."
        });

        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

        options.CustomSchemaIds(type => type.FullName);
    }
}
