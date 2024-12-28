using Common.Endpoints.Hateoas;
using Common.Infrastructure.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Modules.Bookings.Application;
using Modules.Bookings.Endpoints.Bookings;

namespace Modules.Bookings.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the Bookings module endpoints service installer.
/// </summary>
public class EndpointsServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration) => 
        services
            .AddScoped<IHateoasEnricher<BookingDto>, BookingHateoasEnricher>() // More generic way to register these?
            .AddControllers()
            .AddApplicationPart(Endpoints.AssemblyReference.Assembly);
}