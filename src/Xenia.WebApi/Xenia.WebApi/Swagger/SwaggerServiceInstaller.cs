using Common.Infrastructure.Configuration;

namespace Xenia.WebApi.Swagger;

/// <summary>
/// Represents the swagger service installer.
/// </summary>
internal sealed class SwaggerServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    void IServiceInstaller.Install(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<SwaggerGenOptionsSetup>();

        services.ConfigureOptions<SwaggerUiOptionsSetup>();

        services.AddSwaggerGen();
    }
}
