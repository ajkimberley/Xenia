using Common.Infrastructure.Configuration;

using FluentValidation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Utilities.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the Bookings module endpoints service installer.
/// </summary>
public class EndpointsServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration) => 
        services
            .AddControllers()
            .AddApplicationPart(Endpoints.AssemblyReference.Assembly);
}