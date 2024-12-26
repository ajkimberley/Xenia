using Common.Infrastructure.Configuration;
using Common.Persistence.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Persistence;

/// <summary>
/// Represents the persistence service installer.
/// </summary>
internal sealed class PersistenceServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddMemoryCache()
            .ConfigureOptions<ConnectionStringSetup>();
}