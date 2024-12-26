using Common.Infrastructure.Configuration;
using Common.Infrastructure.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.HotelAdmin.Infrastructure;

public class HotelAdminModuleInstaller : IModuleInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration) => 
        services
            .InstallServicesFromAssemblies(configuration, AssemblyReference.Assembly)
            .AddTransientAsMatchingInterfaces(AssemblyReference.Assembly)
            .AddTransientAsMatchingInterfaces(Application.AssemblyReference.Assembly)
            .AddTransientAsMatchingInterfaces(Endpoints.AssemblyReference.Assembly)
            .AddTransientAsMatchingInterfaces(IntegrationEvents.AssemblyReference.Assembly)
            .AddTransientAsMatchingInterfaces(Persistence.AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(Application.AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(Endpoints.AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(IntegrationEvents.AssemblyReference.Assembly)
            .AddScopedAsMatchingInterfaces(Persistence.AssemblyReference.Assembly);
}