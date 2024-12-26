using Common.Infrastructure.Configuration;

using FluentValidation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.HotelAdmin.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the Bookings module application service installer.
/// </summary>
public class ApplicationServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddMediatR(c => c.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly))
            .AddValidatorsFromAssembly(Application.AssemblyReference.Assembly, includeInternalTypes: true);
}